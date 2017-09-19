# Add variables for compiler and cflags
# Assign the values Dustin Shiozaki cs3505 A2
CC = g++
CFLAGS = -Wall -std=c++11

TrieTest: TrieTest.cpp
	$(CC) $(CFLAGS) g++ -o TrieTest TrieTest.cpp

clean:
	rm TrieTest *.o

test:
	./TrieTest 
