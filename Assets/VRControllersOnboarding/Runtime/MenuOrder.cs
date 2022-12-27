
public static class MenuOrder
{
    private const int IntegerDifferenceRequiredForEditorToRenderSeparator = 10; //this is the (odd) way for unity editor to render separator line

    public const int Guidance = 0;
    public const int CompletionRules = Guidance + IntegerDifferenceRequiredForEditorToRenderSeparator + 1;
    public const int Examples = CompletionRules + IntegerDifferenceRequiredForEditorToRenderSeparator + 1;
    public const int Default = Examples + IntegerDifferenceRequiredForEditorToRenderSeparator + 1;

}