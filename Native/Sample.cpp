#include "stdafx.h"

#include <opencv2/core/core.hpp>
#include <opencv2/highgui/highgui.hpp>
#include <iostream>

using namespace cv;

extern "C" DLL_PUBLIC int Sample_Call(int a)
{
	return a * a;
}