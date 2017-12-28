CC = g++
CFLAGS = -Wall -std=c++11
GOOGLETEST ?= googletest/googletest

target:
	$(CC) $(CFLAGS) -o students_tests Students.o StudentsTests.cpp -I $(GOOGLETEST)/include -L $(GOOGLETEST) -lgtest -lgtest_main -lpthread

clean:
	rm students_tests

test:
	./students_tests
