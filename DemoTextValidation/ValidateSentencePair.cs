using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DemoTextValidation
{
    static class ValidateSentencePair
    {
        static char[] Sep = { ' ' };
        static HashSet<string> ValidTags = new HashSet<string> { "<Tag1>", "<Tag2>", "<Tag3>" };
        public static void ValidatePairSentence(string hightGerman, string swissGerman)
        {
            #region 20210414 validation

            var tokens1 = ValidateSingleSentence(hightGerman, "High German").ToArray();            
            var tokens2 = ValidateSingleSentence(swissGerman, "Swiss German").ToArray();

            // If the tokens have passed the check in previous step, then only valid token will be selected in this step.
            var tags1 = tokens1.Where(x => x[0] == '<').ToArray();
            var tags2 = tokens2.Where(x => x[0] == '<').ToArray();
            // The tokens on both size should match each other.
            // For example:
            //  (HG):   abc <Tag1> def <Tag2> gh ijk <Tag1> lm
            //  (SG):   AB <Tag1> DEFG <Tag2> HI JK <Tag1> LMN            
            // The the two sequnces of tags on both sides should be:
            //  (HG):   <Tag1> <Tag2> <Tag1>
            //  (SG):   <Tag1> <Tag2> <Tag1>
            // If they are not matched, then validation fails.
            Sanity.Requires(tags1.SequenceEqual(tags2), "Tag mismatches between High German and Swiss German.");

            int nonTagTokenLength1 = tokens1.Length - tags1.Length;
            int nonTagTokenLength2 = tokens2.Length - tags2.Length;

            // The word count should not different too much.
            // The current thrshold is:
            //  The long sentence should not be more than 1.5 times of the short one.
            //  Or the long sentence should not be more than 5 tokens of the short one.
            Sanity.Requires(nonTagTokenLength1 * 1.5 >= nonTagTokenLength2 || nonTagTokenLength2 - nonTagTokenLength1 <= 5, $"Token count in Swiss German({nonTagTokenLength2}) is much less than High German({nonTagTokenLength1}).");
            Sanity.Requires(nonTagTokenLength2 * 1.5 >= nonTagTokenLength1 || nonTagTokenLength1 - nonTagTokenLength2 <= 5, $"Token count in High German({nonTagTokenLength1}) is much less than Swiss German({nonTagTokenLength2}).");

            #endregion

            /*
             Other validations, maybe in future.
             */
        }

        private static IEnumerable<string> ValidateSingleSentence(string s, string sentenceType)
        {
            var tokens = s.Split(Sep, StringSplitOptions.RemoveEmptyEntries);
            foreach(string token in tokens)
            {
                // Valid tag is allowed.
                // Since tag may contain special chars, the validation of tag should be the highest priority.
                if (ValidTags.Contains(token))
                {
                    yield return token;
                    continue;
                }

                // Digits are not allowed.
                foreach (char c in token)
                {
                    Sanity.Requires(c < '0' || c > '9', $"Digit in {sentenceType}: {token}");
                }

                // If token is not a tag, then '<' or '>' is not allowed.
                // It might be:
                //      abc<Tag1>   : space is required.
                //      <Tag1       : incomplete tag.
                //      <Teg1>      : misspell of tag.
                //      ab<c        : simply typo.
                Sanity.Requires(!token.Contains('<') && !token.Contains('>'), $"Invalid tag in {sentenceType}: {token}");

                // Other tokens are allowed.
                yield return token;
            }
        }        
    }
}
