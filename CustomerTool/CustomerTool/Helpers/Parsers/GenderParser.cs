using CustomerTool.Models;

namespace CustomerTool.Helpers.Parsers
{
    public static class GenderParser
    {
        public static char? FromEnumToChar(Gender gender)
        {
            return (gender) switch
            {
                Gender.Male => 'M',
                Gender.Female => 'F',
                Gender.Other => 'O',
                Gender.Undeclared or _ => null,
            };
        }

        public static Gender FromCharToEnum(char? gender)
        {
            return (gender) switch
            {
                'M' or 'm' => Gender.Male,
                'F' or 'f' => Gender.Female,
                'O' or 'o' => Gender.Other,
                _ => Gender.Undeclared
            };
        }
    }
}
