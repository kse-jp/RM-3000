// dllmain.cpp : DLL アプリケーションのエントリ ポイントを定義します。
#include "stdafx.h"

#include <sstream>

#include "Tool.h"

using namespace std ;

////////////////////////////////////////////////////////////////////////////
#pragma warning( disable : 4996 )
#define _CRT_SECURE_NO_DEPRECATE

#ifdef  _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES
#undef  _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES
#define _CRT_SECURE_CPP_OVERLOAD_STANDARD_NAMES 1
//strcopyやsprintf()などの(バッファオーバラン対策)ワーニングを抑止する
#endif
////////////////////////////////////////////////////////////////////////////


BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}


int Tool_ChgUpper(std::string strS, std::string &strD)
{
	const char *tmp = strS.c_str();
	std::ostringstream upper;

	//大文字に変換
	for (int j = 0; j < (int)strlen(tmp); j++) {
		upper << (char)toupper(tmp[j]);
	}
	strD = upper.str(); 
	return 0;
}

int Tool_ChgUpperChar(const char *input, char *output)
{
	std::ostringstream upper;

	strcpy(output, input);
	//大文字に変換
	for (int j = 0; j < (int)strlen(input); j++) {
		output[j] = (char)toupper(input[j]);
	}
	return 0;
}



std::string strTokenBuf[MAX_TOKEN_STRINGS];

unsigned int stringToken(
					std::string strS,		//区切られる文字列
					char *tkn_str			//区切り文字		",;\n"など
					)
{
	// 区切られる文字列
	const char *gstr_ = strS.c_str();

	int k;
	int str_count = 0;						// 実際に読み込まれたトークンの数
	for(k = 0; k < MAX_TOKEN_STRINGS; k++){
		strTokenBuf[k] = ""; 
	}

	char *data[MAX_TOKEN_STRINGS];			// 区切り文字で区切られた個々の文字列(トークン)が入る

	data[0] = strtok((char*)gstr_, tkn_str);
	for(k = 1; (data[k] = strtok(NULL, tkn_str)) != NULL; k++) ;

	str_count = k;							// ここでの k の値が読み込まれたトークンの数になる

	for(k = 0; k < str_count; k++){
		if( k > MAX_TOKEN_STRINGS ){
			break;
		}
		strTokenBuf[k] = data[k];
		//printf("stringToken >>>>> data[%d] : %s\n", k, data[k]);
	}

	return str_count;
}

//ADD 2010-03-19
LARGE_INTEGER Performance_time1;
LARGE_INTEGER Performance_time2;
LARGE_INTEGER Performance_freq;

LARGE_INTEGER Timer_Start(void)
{
	QueryPerformanceCounter( &Performance_time1 );
	return Performance_time1;
}
LARGE_INTEGER Timer_Stop(double *timeVal)
{
	QueryPerformanceCounter( &Performance_time2 );	// 計測終了！

	QueryPerformanceFrequency( &Performance_freq);  // カウンタ周波数(3579545Hz)の取得

	double time = (double)(Performance_time2.QuadPart - Performance_time1.QuadPart) / (double)Performance_freq.QuadPart;
	time *= 1000;

	*timeVal = time;
	return Performance_time2;
}
//ADD 2010-03-19
