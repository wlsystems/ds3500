# Add variables for compiler and cflags
# Assign the values Dustin Shiozaki cs3505 A2
CC = g++
CFLAGS = -Wall -std=c++11

spiralPDF: spiralPDF.cpp
	$(CC) $(CFLAGS) -o spiralPDF spiralPDF.cpp -L src -I include libhpdfs.a -lm -lz


test:
	./SpiralPDF "this is my sample text"
