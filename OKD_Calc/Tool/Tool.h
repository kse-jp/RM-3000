/////////////////////////////////////////////////////////////////////
//
//  Tool.h
//  ツールクラス
// 
//	2009-10-08	作成	中岡昌俊


#ifndef __TOOL_H
#define __TOOL_H

#include <string>
#include <iterator>
#include <vector>
#include <iostream>


int Tool_ChgUpper(std::string strS, std::string &strD);
int Tool_ChgUpperChar(const char *input, char *output);

unsigned int stringToken(
					std::string strS,		//区切られる文字列
					char *tkn_str			//区切り文字		",;\n"など
					);

#define MAX_TOKEN_STRINGS 300
extern std::string strTokenBuf[MAX_TOKEN_STRINGS];

#endif

//ADD 2010-03-19
LARGE_INTEGER  Timer_Start(void);
LARGE_INTEGER  Timer_Stop(double *timeVal);
//ADD 2010-03-19
