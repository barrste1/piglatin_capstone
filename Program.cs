using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace _07232020_piglatin_capstone
{
    class Program
    {
        static void Main(string[] args)
        {   //boolend used to allow repeats of game from running script. ContinuePlay method ends this loop.
            bool end = false;
            while (end == false)
            {
                //array of punctuation. Allows words to still be translated to pig latin if they end with punctuation
                char[] punt = { '!', '.', '?', ',' };
                //vowel is an array of all capital and lowercase vowels. This allows the input to be case sensitive
                //and hold those capital values.
                char[] vowel = { 'a', 'e', 'i', 'o', 'u'};
                string output = "";
                MakeGreen("Please give me a word! Or Sentence. You know. If you're feeling adventurous.");
                //inputs will return valid if there is anything written (not blank space), and if no punctuation in
                //the middle of words. If refining, this is the first place to check.
                string input = ValidateText(Console.ReadLine());
                //This line removes additional whitespace and creates array for each word in a string to be looked at
                //in isolation by the foreach loop below. 
                string[] words = input.Split(" ", StringSplitOptions.RemoveEmptyEntries);
                foreach (string word in words)
                {
                    string wordLow = word.ToLower();
                    //numword allows TryParse on line 41. This way only substrings that are purely numbers are caught
                    //by that conditional. this allows dr4gon to still be pig-latinized
                    int numword = 0;
                    //Array vowel has all lowercase vowels, so passing wordLow, the To.Lower of original input, to have
                    //an index location for the first vowel helps determine whether to add "ay" or "way". Important!
                    int MakePigLatin = wordLow.IndexOfAny(vowel);
                    if (MakePigLatin == -1)
                    {   //This is important for handling words like "why" or "fly", which have no vowel, as defined by
                        //the rules of the assignment. No need for CheckCapital as word is already case sensitive.
                        //no changes are made to words that end up here. 
                        output += $" {word}";
                        Console.Beep(1200, 50); Console.Beep(1200, 50); Console.Beep(1200, 50); Console.Beep(1200, 50);
                    }
                    else if (word.Contains('@') || int.TryParse(word, out numword) == true)
                    {   //This is for covering e-mails and numbers. if the string has an @ then it is an e-mail, and is
                        //posted as is. if it can parse to a number then it is a number, and should get posted as well
                        //no manipulation required for extended exercise.
                        output += $" {word}";
                        Console.Beep(400, 200);
                    }
                    else
                    {   //Removing punctuation, to allow string manipulation. Punctuation is then added back on
                        //afterward.
                        string puntRemoveLower = wordLow.TrimEnd(punt);
                        string puntRemove1 = word.TrimEnd(punt);
                        //wordlen is the position of the original string where the punctuation starts at
                        int wordlen = puntRemoveLower.Length;
                        //by appending word.Substring(wordlen) on the end it will append all characters from the first
                        //punctuation to the end of the string, no matter how many "..." or !!! there are.
                        //CheckVowel will determine if the first character is a vowel, and manipulate it accordingly
                        //(if vowel first then simply append "way", if consonant then move letters over and add "ay")
                        //puntremoveLower and puntRemove are sent to CheckVowel so that punctuation isn't kept in the 
                        //middle of the word.
                        output += $" {Program.MakePigLatin(puntRemoveLower, MakePigLatin, puntRemove1)}{word.Substring(wordlen)}";
                    }
                }
                Console.Beep(600, 100); Console.Beep(300, 800);
                //output.TrimStart is used to remove the initial whitespace that is used in when appending on new words in 
                //our string. program is not built to preserve the amount of spaces between words.
                MakeCyan(output.TrimStart());
                //If "N" entered, end returns as true, ending the while loop on line 17. If "y", then end stays = false.
                end = ContinuePlay("Do you want to see more pig latin? (Y/N)");
            }
        }
        public static bool ContinuePlay(string message)
        {//This either ends the play loop or allows someone to enter another input
            bool end = false;
            string cont = "";
            MakeGreen(message);
            while (end==false)
            {
                cont = Console.ReadLine().ToLower();
                if (cont == "y")
                {
                    break;
                }
                else if (cont == "n")
                {
                    end = true;
                }
                else
                {
                    MakeGreen("Invalid input. Please enter (y) to play again or (n) to stop.");
                }
            }
            return end;
        }
        public static string ValidateText(string message)
        {   //this method will validate that there is only puntuation marks at the end of the word
            //it will also validate that the input is not empty. message.Trim() is implemented so that if the user
            //types multiple spaces it will be counted as blank as well.
            char[] punt = { '!', '.', '?',',' };
            bool valid = false;
            while (valid == false)
            {
                valid = true;
                while (message.Trim() == "")
                {
                    MakeGreen("Aww play along! Give me a word or sentence!");
                    message = Console.ReadLine();
                }
                string[] words = message.Split();
                foreach (string word in words)
                {   //puntRemove removes punctuation from the end of the word; puntcuation at the end of word is allowed in sentences
                    string puntRemove = word.TrimEnd(punt);
                    if (word.Contains('@'))
                    {   //e-mails are allowed, as they have @ symbols in them and this part of the if should check first.
                        //punctuation in the middle of an e-mail is allowable as it is not piglatinized
                        continue;
                    }
                    else if (puntRemove.Contains('!') == true || puntRemove.Contains('?') == true || puntRemove.Contains('.') == true)
                    {
                        valid = false;
                        MakeGreen("Sentences should end with punctuation. Please input a sentence that follows grammar rules");
                        message = Console.ReadLine();
                    }

                }
            }
            return message;
        }
        public static void MakeGreen(string message)
        {   //makes text the console prints green, while user entered text is white. 
            //this looks nice. that is all!
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static void MakeCyan(string message)
        {   //pig latin output is the only message that uses this. it is special, and should stand out.
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }
        public static string MakePigLatin(string message, int vowel,string messageCase)
        {//checks if the word starts with a vowel, and returns the pig latin version of the word
            string output = "";
            if (vowel == 0)
            {   //messageCase is used, as no manipulation is really done to original word other than adding "way"
                output = messageCase + "way";
                Console.Beep(500, 75); Console.Beep(600, 75); Console.Beep(500, 75);Console.Beep(300, 200);
            }
            else
            {   //pig latin word is build by substrings, and sent to CheckCapital to capitalize letters. vowel here is
                //first vowel index location, so that is where the cut is being made. 
                output = (message.Substring(vowel) + message.Substring(0, vowel));
                output = $"{CheckCapital(output, messageCase)}ay";
                Console.Beep(600, 75);
            }
            return output;
        }
        public static string CheckCapital(string lowerMessage,string messageCase)
        {   //charPlace increments with each loop, and is used to inch along word (second iteration
            //of loop compares position 1, etc.). word.To.Lower is passed in lowerMessage, and messageCase is original
            //case sensitive word. if messageCase is capitalized in a position then the corresponding position is
            //capitalized in the lowercase message. this keeps the message case sensitive while still have To.Lower 
            //used in the assignment (rubric)
            string output = "";
            int charPlace = 0;
            foreach(char c in messageCase)
            {
                if (char.IsUpper(c))
                {
                    string upIt = lowerMessage.Substring(charPlace, 1);
                    output += upIt.ToUpper();
                }
                else if (char.IsLower(c))
                {
                    output += lowerMessage.Substring(charPlace, 1);
                }
                else
                {
                    output += lowerMessage.Substring(charPlace, 1);
                }
                charPlace++;
            }
            return output;
        }

    }
}