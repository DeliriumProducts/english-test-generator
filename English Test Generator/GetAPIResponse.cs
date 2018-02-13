using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GetAPIResponse
{
    enum LexicalCategory
    {
        Verb, Noun, Adjective
    }


    class Definitions
    {
        public static void get(LexicalCategory category, string word)
        {
            if (category == LexicalCategory.Noun)
            {
                System.Windows.Forms.MessageBox.Show("vlqzoh v sistemata");
            }
        }

        public static void get(string category, string word)
        {
            get(map.FirstOrDefault(x => x.Value == category).Key, word);
        }

        public static string categoryString(LexicalCategory category)
        {
            return map[category];
        }

        public static LexicalCategory categoryFromString(string category)
        {
            return map.FirstOrDefault(x => x.Value == category).Key;
        }

        public static Dictionary<LexicalCategory, string> map =
            new Dictionary<LexicalCategory, string>
            {
                { LexicalCategory.Verb, "Verb"},
                { LexicalCategory.Noun, "Noun"},
                { LexicalCategory.Adjective, "Adjective" }
            };
    }
    class Examples
    {
        public void Test()
        {
          

        }
    }
}
