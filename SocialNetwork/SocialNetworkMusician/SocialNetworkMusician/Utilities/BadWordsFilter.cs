namespace SocialNetworkMusician.Utilities
{
    public static class BadWordsFilter
    {
        public static readonly List<string> BadWords = new()
        {
            "arse", "arsehead", "arsehole", "ass", "ass hole", "asshole", "bastard", "bitch", "bloody", "bollocks", "brotherfucker", "bugger", "bullshit",
            "child-fucker", "Christ on a bike", "Christ on a cracker", "cock", "cocksucker", "crap", "cunt", "dammit", "damn", "damned", "damn it", "dick",
            "dick-head", "dickhead", "dumb ass", "dumb-ass", "dumbass", "dyke", "faggot", "father-fucker", "fatherfucker", "fuck", "fucked", "fucker", "fucking",
            "god dammit", "goddammit", "God damn", "god damn", "goddamn", "Goddamn", "goddamned", "goddamnit", "godsdamn", "hell", "holy shit", "horseshit", "in shit",
            "jackarse", "jack-ass", "jackass", "Jesus Christ", "Jesus fuck", "Jesus Harold Christ", "Jesus H. Christ", "Jesus, Mary and Joseph", "Jesus wept", "kike",
            "mother fucker", "mother-fucker", "motherfucker", "nigga", "nigra", "pigfucker", "piss", "prick", "pussy", "shit", "shit ass", "shite", "sibling fucker",
            "sisterfuck", "sisterfucker", "slut", "son of a bitch", "son of a whore", "spastic", "sweet Jesus", "twat", "wanker"
        };

        public static string CleanComment(string input)
        {
            foreach (var word in BadWords)
            {
                var pattern = @"\b" + System.Text.RegularExpressions.Regex.Escape(word) + @"\b";
                input = System.Text.RegularExpressions.Regex.Replace(input, pattern, "***", System.Text.RegularExpressions.RegexOptions.IgnoreCase);
            }
            return input;
        }
    }
}
