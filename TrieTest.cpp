/*Dustin Shizoaki A3 u0054455

*/
#include <iostream>
#include <fstream>
#include <stdlib.h>
using namespace std;
#include "Trie.cpp"

int main(int argc, char **argv){
	ifstream inFile;
	inFile.open(argv[1]);
	if (!inFile) {
    	cerr << "Unable to open file";
    	exit(1);   // call system to stop
	}
	string line;
	Trie tr;
	if (inFile.is_open())
  	{
    	while (getline (inFile,line))
	    {
	      	tr.addWord(line);
	    }
    inFile.close();
  	}

  	inFile.open(argv[2]);
  	if (inFile.is_open())
  	{
  		while (getline (inFile,line))
	    {

	      	std::vector<std::string> pwords;
	      	pwords = tr.getPossWords(line);
	      	if  (line.size() == 0)	{
	      		tr.current = tr.getSuffixTrie(line);  		
	      		std::vector<std::string> words;
	      		words = tr.allWordsStartingWithPrefix(line);
	      		std::vector<string>::iterator it;
	      		cout<<line<<" is not found, did you mean:"<<endl;
	      		for (it=words.begin(); it < words.end(); it++)
	      			cout<<"   "<<*it<<endl;

	      	}

	      	else if (pwords.size() >0) {
	      		cout<<"wildcard Found"<<endl;
	      	}
	      		      		
	      	else if (tr.isWord(line))
	      		cout<<line<<" is found"<<endl;
	      	
	      		
	      		
	      	else {

	      		cout<<line<<" is not found, did you mean:"<<endl;
	      		tr.current = tr.getSuffixTrie(line);      		
	      		std::vector<std::string> words;
	      		words = tr.allWordsStartingWithPrefix(line);
	      		std::vector<string>::iterator it;
	      		for (it=words.begin(); it < words.end(); it++)
	      			cout<<"   "<<*it<<endl;
	      	}	    
	    }
    	inFile.close();
  	}
  	//tr.allWordsStartingWithPrefix("i");
	return 0;
}