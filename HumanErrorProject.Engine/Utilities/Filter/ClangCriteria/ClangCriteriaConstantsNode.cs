namespace HumanErrorProject.Engine.Utilities.Filter.ClangCriteria
{
    public class ClangCriteriaConstantsNode : ClangCriteriaNode
    {
        public override bool Pass(string value)
        {
            switch (value)
            {
                case "__int128_t":
                case "__uint128_t":
                case "__NSConstantString":
                case "__builtin_va_list":
                case "__builtin_ms_va_list":
                    return true;
                default:
                    return false;
            };
        }
    }
}
