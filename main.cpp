/*
 * Team Deathstar IT
 * CS3505 - A7: Sprite Editor
 * main.cpp
 *
 *
 */

#include "mainwindow.h"
#include <QApplication>
#include <QDebug>

int main(int argc, char *argv[])
{
	QApplication a(argc, argv);
	Model m;
	MainWindow w(m);
    w.show();

    return a.exec();
}
