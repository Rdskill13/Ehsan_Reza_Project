using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.IMGUI.Controls;
using UnityEngine;
using ImmersiveVRTools.Editor.Common.GenericTable;
using ImmersiveVRTools.Editor.Common.Utilities;
using ImmersiveVRTools.Runtime.Common.Utilities;
using VRControllersOnboarding.Runtime;

namespace VRControllersOnboarding.Editor
{
    public class ChangeUnityXRMappingsEditorWindow : EditorWindow
    {
	    [NonSerialized] bool _isTreeViewInitialized;
	    [SerializeField] TreeViewState TreeViewState; // Serialized in the window layout file so it survives assembly reloading
	    [SerializeField] MultiColumnHeaderState MultiColumnHeaderState; //TODO: does that need to be here for serialization to work? what is even that serialization helping with?
	    private GenericTableTreeView<ControllerMappings, ControllerButtonMappingSet> _treeView;
	    
        private ButtonMappingToOnboardingSetup CurrentButtonMappingToOnboardingSetup { get; set; }
        private SerializedProperty CurrentButtonMappingToOnboardingSetupSerializedProperty { get; set; }
        private SerializedObject CurrentlySerializedObject { get; set; } 
        
        public void Init(SerializedProperty currentButtonMappingToOnboardingSetupSerializedProperty, SerializedObject serializedObject)
        {
	        minSize = new Vector2(1600, 450);
	        _isTreeViewInitialized = false;
            
            CurrentlySerializedObject = serializedObject;
            CurrentButtonMappingToOnboardingSetupSerializedProperty = currentButtonMappingToOnboardingSetupSerializedProperty;
            CurrentButtonMappingToOnboardingSetup = (ButtonMappingToOnboardingSetup)currentButtonMappingToOnboardingSetupSerializedProperty.serializedObject.targetObject;
        }
        
        void OnGUI()
        {
	        if (CurrentlySerializedObject == null)
            {
                GUI.enabled = false;
                EditorGUILayout.TextArea("No Object found, please reopen the window by clicking 'Change/Preview UnityXR Mappings' button");
                GUI.enabled = true;
                
                return;
            }
	        
	        InitIfNeeded();
            
            CommonEditorUI.InfoMessage(
@"This table will help you choose from existing example actions while showing you the mapping bindings for different input systems / frameworks. 

Some actions/buttons are not mapped as they are not used in demo - you can very easily create those actions so they are tailored to your specific application.

Have a look at the docs for easy-to-follow guide."
            );
            //TODO: add button that takes user to docs online
            
            CurrentlySerializedObject.Update();
            
            EditorGUILayout.PropertyField(CurrentButtonMappingToOnboardingSetupSerializedProperty);

            var lastRect = GUILayoutUtility.GetLastRect();
            const int additionalSpacing = 20;
            lastRect.y += additionalSpacing;
            lastRect.width = position.width;
            lastRect.height = position.height - (lastRect.y + additionalSpacing);
            _treeView.OnGUI(lastRect);
            
            CurrentlySerializedObject.ApplyModifiedProperties();
        }
        
        // Rect multiColumnTreeViewRect
        // {
	       //  get { return new Rect(20, 30, position.width-40, position.height-60); }
        // }
        
        void InitIfNeeded ()
        {
            if (!_isTreeViewInitialized)
            {
                // Check if it already exists (deserialized from window layout file or scriptable object)
                if (TreeViewState == null)
                    TreeViewState = new TreeViewState();

                var allControllerButtonMappingSets = AssetDatabaseHelper.GetAllScriptableObjects<ControllerButtonMappingSet>();
                _treeView = new UnityXrMappingTableTreeViewBuilder(
	                new GenericTableTreeViewState(TreeViewState, MultiColumnHeaderState),
	                allControllerButtonMappingSets,
	                CurrentlySerializedObject,
	                CurrentButtonMappingToOnboardingSetupSerializedProperty
	            ).Create();
                
                 _isTreeViewInitialized = true;
            }
        }
    }

    internal class UnityXrMappingTableTreeViewBuilder : GenericTableTreeViewBuilder<ControllerMappings, ControllerButtonMappingSet>
    {
	    private readonly List<ControllerButtonMappingSet> _allControllerButtonMappingSets;
	    private readonly SerializedProperty _selectedMappingSetProperty;
	    
	    private readonly string _actionTypeHeader = "Action Type";
	    
	    private GUIStyle _unityXRMappingsDelimiterStyle;
	    void InitStyles()
	    {
		    if (_unityXRMappingsDelimiterStyle == null || _unityXRMappingsDelimiterStyle.normal.background == null)
		    {
			    _unityXRMappingsDelimiterStyle = new GUIStyle(GUI.skin.label);
			    _unityXRMappingsDelimiterStyle.normal = new GUIStyleState()
				    {background = TextureHelper.CreateGuiBackgroundColor(new Color(0.0f, 1f, 0.0f, 0.2f))};
			    _unityXRMappingsDelimiterStyle.normal.textColor = Color.white;
			    _unityXRMappingsDelimiterStyle.fontStyle = FontStyle.Bold;
		    }
	    }


	    public UnityXrMappingTableTreeViewBuilder(GenericTableTreeViewState state, List<ControllerButtonMappingSet> allControllerButtonMappingSets, 
		    SerializedObject onboardingSetupRootObject,
		    SerializedProperty selectedMappingSetProperty) 
		    : base(state, allControllerButtonMappingSets)
	    {
		    _allControllerButtonMappingSets = allControllerButtonMappingSets;
		    _selectedMappingSetProperty = selectedMappingSetProperty;
		    InitStyles();
	    }
	    
	    protected override List<GenericTableTreeViewColumnData<ControllerMappings>> GetColumnData()
	    {
		    var allControllerMappingTypes = _allControllerButtonMappingSets
			    .SelectMany(s => s.UnityXRControllerButtonMappingEntries)
			    .Select(s => s.UnityXRControllerMappingsRoot)
			    .OrderBy(s => s.name)
			    .Distinct()
			    .ToList();
		    
		    return new List<GenericTableTreeViewColumnData<ControllerMappings>>()
		    {
			    new GenericTableTreeViewColumnData<ControllerMappings>(new StaticColumnData<ControllerButtonMappingSet>(_actionTypeHeader, 200, null)),
			    new GenericTableTreeViewColumnData<ControllerMappings>(new StaticColumnData<ControllerButtonMappingSet>("Hand", 70,
				    (rowEntry, cellRect) =>
				    {
					    EditorGUI.LabelField(cellRect, rowEntry.Data.Handednes.ToString()); 
				    })),
			    new GenericTableTreeViewColumnData<ControllerMappings>(new StaticColumnData<ControllerButtonMappingSet>("New Input System", 330,
				    (rowEntry, cellRect) =>
				    {
					    EditorGUI.LabelField(cellRect, rowEntry.Data.NewInputSystemActionFullPath.ToString()); 
				    })),
			    new GenericTableTreeViewColumnData<ControllerMappings>(new StaticColumnData<ControllerButtonMappingSet>("Steam VR", 220,
				    (rowEntry, cellRect) =>
				    {
					    EditorGUI.LabelField(cellRect, rowEntry.Data.SteamVRActionFullPath.ToString()); 
				    })),
			    new GenericTableTreeViewColumnData<ControllerMappings>(new StaticColumnData<ControllerButtonMappingSet>("OVR Button", 165,
				    (rowEntry, cellRect) =>
				    {
					    EditorGUI.LabelField(cellRect, rowEntry.Data.OVRButtonName.ToString()); 
				    })),
			    new GenericTableTreeViewColumnData<ControllerMappings>(new StaticColumnData<ControllerButtonMappingSet>("UnityXR ->", 80,
				    (rowEntry, cellRect) =>
				    {
					    GUI.Box(cellRect, GUIContent.none, _unityXRMappingsDelimiterStyle); 
				    }))
		    }.Concat(
				allControllerMappingTypes.Select(t => new GenericTableTreeViewColumnData<ControllerMappings>(t))
			).ToList();
	    }

	    protected override MultiColumnHeaderState.Column CreateColumn(GenericTableTreeViewColumnData<ControllerMappings> columnData)
	    {
		    var staticColumnData = columnData.NonGenericColumnData as StaticColumnData<ControllerButtonMappingSet>;
		    if (staticColumnData != null)
		    {
			    return GenerateDefaultColumn(staticColumnData.Header, staticColumnData.ColumnWidth);
		    }
		    else if(columnData.Data)
		    {
			    return GenerateDefaultColumn(columnData.Data.name, 100);
		    }
		    else
		    {
			    return new MultiColumnHeaderState.Column
			    {
				    headerContent = new GUIContent("Invalid setup"),
			    };
		    }
	    }

	    protected override void RenderCell(GenericTableItem<ControllerButtonMappingSet> rowEntry, GenericTableTreeViewColumnData<ControllerMappings> columnData, Rect cellRect)
	    {
		    //TODO: not ideal to do it like that with 'static' columns, perhaps it'd be better to have renderFn coming from single method where both header and cell rendering would be specified in one place? Potentially heavy on performance?
		    var staticColumnData = columnData.NonGenericColumnData as StaticColumnData<ControllerButtonMappingSet>;
		    if (staticColumnData != null)
		    {
			    if (staticColumnData.Header == _actionTypeHeader)
			    {
				    var selected = _selectedMappingSetProperty.objectReferenceValue == rowEntry.Data;
				    if (EditorGUI.Toggle(cellRect, selected))
				    {
					    _selectedMappingSetProperty.objectReferenceValue = rowEntry.Data;
				    }

				    cellRect.x += 15;
				    EditorGUI.LabelField(cellRect, rowEntry.Data.name); 
			    }
			    else if (staticColumnData.RenderCellFn != null)
			    {
				    staticColumnData.RenderCellFn(rowEntry, cellRect);
			    }
			    else
			    {
				    EditorGUI.LabelField(cellRect, "Invalid setup"); 
			    }
		    }
		    else if (columnData.Data)
		    {
			    var matching = rowEntry.Data.UnityXRControllerButtonMappingEntries.FirstOrDefault(e => e.UnityXRControllerMappingsRoot == columnData.Data);
			    var displayName = matching == true ? matching.name : "Not mapped";
			    if (matching != null)
			    {
				    EditorGUI.LabelField(cellRect, displayName); 
			    }
		    }
		    else
		    {
			    EditorGUI.LabelField(cellRect, "Invalid setup"); 
		    }
	    }
	    
	    private static MultiColumnHeaderState.Column GenerateDefaultColumn(string header, int? forceWidth)
	    {
		    return new MultiColumnHeaderState.Column
		    {
			    headerContent = new GUIContent(header),
			    headerTextAlignment = TextAlignment.Left,
			    canSort = false,
			    width = forceWidth ?? 100,
			    minWidth = forceWidth ?? 60,
			    autoResize = false,
			    allowToggleVisibility = false
		    };
	    }
    }

    internal class StaticColumnData<TRowData>
    {
	    public Action<GenericTableItem<TRowData>, Rect> RenderCellFn { get; }
	    public string Header { get; }
	    public int ColumnWidth { get; set; }

	    public StaticColumnData(string header, int columnWidth, Action<GenericTableItem<TRowData>, Rect> renderCellFn)
	    {
		    RenderCellFn = renderCellFn;
		    Header = header;
		    ColumnWidth = columnWidth;
	    }
    }
}