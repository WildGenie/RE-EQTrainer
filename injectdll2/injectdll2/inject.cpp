// REFERENCES and RESOURCES
// =======================
// Auto Unload DLL: http://www.unknowncheats.me/forum/613953-post43.html
// EQ Mac Functions: https://github.com/sodcheats/eqmac/blob/master/eqmac/include/eqmac.hpp
// Find warp tutorial - http://www.redguides.com/forums/showthread.php/4846-Simplest-offset-to-find-ever-(Warp-offset)
// Find safe xyz - http://eqpathfinders.guildportal.com/Guild.aspx?GuildID=9508&TabID=77837&ForumID=614583&TopicID=6833250

#include "windows.h"
#include "Stdafx.h"
#include <string>
using namespace std;

void EQTFunctions (const char *func) {
	if(strcmp("warp",func) != 0){
		typedef void (__thiscall* CGCamera__ResetView)();
		CGCamera__ResetView ResetView = (CGCamera__ResetView)0x0043D7C5; //TITANIUM = 0x0043D7C5   //MAC = 0x004B459C
		ResetView();
	}
}

void OnAttach( HMODULE hModule ) {

	HANDLE hPipe;
    char buffer[1024];
    DWORD dwRead;


    hPipe = CreateNamedPipe(TEXT("\\\\.\\pipe\\EQTPipe"),
                            PIPE_ACCESS_DUPLEX/*PIPE_ACCESS_INBOUND*/ | PIPE_TYPE_BYTE | PIPE_READMODE_BYTE,
                            PIPE_WAIT,
                            1,
                            1024 * 16,
                            1024 * 16,
                            NMPWAIT_USE_DEFAULT_WAIT,
                            NULL);
    while (hPipe != NULL)
    {
        if (ConnectNamedPipe(hPipe, NULL) != FALSE)
        {
            while (ReadFile(hPipe, buffer, sizeof(buffer), &dwRead, NULL) != FALSE)
            {
				EQTFunctions(buffer);
            }
        }

        DisconnectNamedPipe(hPipe);
    }

	// old function
	//EQTFunctions();
	//FreeLibraryAndExitThread( hModule, 0 );                               
	//ExitThread( 0 );
}

BOOL APIENTRY DllMain( HMODULE hModule,
                       DWORD  ul_reason_for_call,
                       LPVOID lpReserved
					 )
{
	switch (ul_reason_for_call)
	{
	case DLL_PROCESS_ATTACH:
		CreateThread( NULL, 0, (LPTHREAD_START_ROUTINE)OnAttach, hModule, 0, NULL );            
	case DLL_THREAD_ATTACH:
	case DLL_THREAD_DETACH:
	case DLL_PROCESS_DETACH:
		break;
	}
	return TRUE;
}