/*Dustin Shizoaki A3 u0054455

*/
#include "Trie.h"
#include <cstring>
#include <sstream>
#include <string>

/*default destructor uses unordered_set of previously
visit nodes to avoid testing all 26.

*/
Trie::~Trie(){
	for (auto i = begin(visitedNodes); i != end(visitedNodes); ++i){
		int x = (*i);
		delete t[x];
	}
}

/*sets the bool to false to indicate if the node is 
terminating a word.

*/
Trie::Trie() : t(){
	isEnd = false;
}
/*also uses unordered_set see comment above

*/
Trie::Trie(Trie& other){ 
	isEnd = other.isEnd;
	visitedNodes = other.visitedNodes;
	for (auto i = begin(visitedNodes); i != end(visitedNodes); ++i){
		int x = (*i);
		t[x] = other.t[x];
	}
}
/*also uses unordered_set see comment above

*/
Trie & Trie::operator=(Trie & other){
	std::swap(isEnd, other.isEnd);
	std::swap(visitedNodes, other.visitedNodes);
	for (auto i = begin(visitedNodes); i != end(visitedNodes); ++i){
		int x = (*i);
		std::swap(t[x], other.t[x]);
	}
	return *this;
}		

/*checks if the word is there using basic recurssion

*/
bool Trie::isWord(std::string str){
	bool result;
	current = new Trie(*this); //allocates new memory
	if (t[getVal(str)]){
		if (str.size() == 1){
			if (t[getVal(str)]->isEnd == true){	//check if theressomething there		
				result = true;
			}
			else
				result = false;
			}
		else{//call the child node medthod
			result = current->t[getVal(str)]->isWord(str.substr(1, str.size()));	
		}
	}
	else {
		result = false;
	}
	return result;
}

/*saves the words inside properties for retrieval.

*/
void Trie::addWord(std::string st){
	if (st.size() > 0){
		visitedNodes.insert(getVal(st));
		if (!t[getVal(st)])
			t[getVal(st)] = new Trie();
		if (st.size() == 1){
			t[getVal(st)]->isEnd = true;
		}
		else {
			t[getVal(st)]->parent = this;
			storedWords.insert(st.substr(0,st.size()));//saves the rest of the word for retrieval
			nextLetters.insert(st.substr(0,1));//was going to use for wildcard
			t[getVal(st)]->addWord(st.substr(1,st.size()));//recursion part
		}
	}
}

//helper method
int Trie::getVal(string s){
	const char *word2 = s.c_str();
	char c = word2[0];
	return (int)c-97;
}

//helper method 
std::string Trie::getStr(int x){
	char c = "abcdefghijklmnopqrstuvwxyz" [x];
	std::string s;
	s = (1,c);
	return s;
}

//was going to use for wildcard part
std::vector<std::string> Trie::getPossWords(std::string s){
	std::vector<std::string> pwords;
	if (s.size() == 1)
		return pwords;
	
/*
return all words in set by accessing the unordered_set of storedWords.
*/
}
std::vector<std::string> Trie::allWordsStartingWithPrefix(std::string st){
	std::vector<std::string> results;
	string s;
	for (auto i = begin(current->storedWords); i != end(current->storedWords); ++i){
			s = (*i);
			results.push_back(st+s);
		}
	return results;
}
	
/*
didnt finish
*/
std::vector<std::string> Trie::wordsWithWildcardPrefix(std::string){
	std::vector<std::string> wildWords;
}

Trie * Trie::getSuffixTrie(std::string s){
	current = new Trie(*this);
	if (t[getVal(s)]){
		if (s.size() == 0)
			return this;
		else if (s.size() == 1){
			current = current->t[getVal(s)];
			return current;
		}
		else{
			current = current->t[getVal(s)]->getSuffixTrie(s.substr(1, s.size()));	
			return current;
		}
	}
	else {
		return current;
	}
}



