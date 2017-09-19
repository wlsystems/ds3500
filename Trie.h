/*Dustin Shizoaki A3 u0054455

*/
#include <vector>
#include <unordered_set>

class Trie {
 Trie* t[26];
 Trie* parent;
 bool isEnd;
 std::unordered_set<string> nextLetters;
 std::unordered_set<unsigned int> visitedNodes;
 std::unordered_set<string> storedWords;
 int counter;
public:
  Trie* current; // accessible methods
  // Constructors
  Trie(); // a default constructor.
  Trie(int x, int y); // initializer style
  ~Trie();
  Trie(Trie& other);
  Trie * getSuffixTrie(std::string s);
  Trie & operator=(Trie& other);
  void addWord(std::string st);
  bool isWord(std::string str);
  int getVal(std::string s);
  std::string getStr(int x);
  std::string getWord(string s);
  std::vector<std::string> allWordsStartingWithPrefix(std::string);
  std::vector<std::string> wordsWithWildcardPrefix(std::string);
  std::vector<std::string> getPossWords(std::string);
}; // end of the Point class definition