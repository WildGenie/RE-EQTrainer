// REFERENCES and RESOURCES
// =======================
// Auto Unload DLL: http://www.unknowncheats.me/forum/613953-post43.html
// EQ Mac Functions: https://github.com/sodcheats/eqmac/blob/master/eqmac/include/eqmac.hpp
// Find warp tutorial (titanium and EQMac) - http://www.redguides.com/forums/showthread.php/4846-Simplest-offset-to-find-ever-(Warp-offset)
// Find warp tutorial (underfoot and Live) - http://www.mmobugs.com/forums/everquest-cheats-and-guides/1460-easy-warp-dothezone-fast.html
// Find safe xyz - http://eqpathfinders.guildportal.com/Guild.aspx?GuildID=9508&TabID=77837&ForumID=614583&TopicID=6833250

#include "windows.h"
#include "Stdafx.h"
#include <string>

#include <MQ2Main.h>

using namespace std;

struct TradeRequest_Struct {
	/*00*/	uint32_t to_mob_id;
	/*04*/	uint32_t from_mob_id;
	/*08*/
};

#define OP_TradeRequest 0x372f
#define OP_TradeRequestAck 0x4048
#define OP_TradeAcceptClick 0x0065
#define OP_FinishTrade 0x6014
#define	MOVE	0x14CB
#define MoveLocalPlayerToSafeCoords 0x43D7C5

void EQTFunctions (const char *func, int len) {
	char newText[1024] = "";
	strncpy(newText, func, len);

	// DEBUG
	//wchar_t *text = new wchar_t[len];
	//mbstowcs(text, newText, len);
	//MessageBox(NULL, text, NULL, MB_OK);

	if(strcmp("warp", newText) == 0){ //MoveLocalPlayerToSafeCoords
		//MessageBox(NULL, L"warp", NULL, MB_OK);
		typedef void (__thiscall* CGCamera__ResetView)();
		CGCamera__ResetView ResetView = (CGCamera__ResetView)MoveLocalPlayerToSafeCoords; //TITANIUM = 0x0043D7C5   //MAC = 0x004B459C  //UNDERFOOT = 0x00499CE8?
		ResetView();
	}
	if (strcmp("opentrade", newText) == 0) {
		//MessageBox(NULL, L"opentrade", NULL, MB_OK);
		PSPAWNINFO pChar;
		PSPAWNINFO pMyTarget = (PSPAWNINFO)pTarget;

		TradeRequest_Struct trStruct;
		trStruct.to_mob_id = pMyTarget->SpawnID;
		trStruct.from_mob_id = pChar->SpawnID;

		SendEQMessage(OP_TradeRequest, &trStruct, sizeof(trStruct));
		SendEQMessage(OP_TradeRequestAck, &trStruct, sizeof(trStruct));
	}
}

void OnAttach( HMODULE hModule ) {

	HANDLE hPipe;
    char buffer[1024];
	char text[1024];
    DWORD dwRead;

    hPipe = CreateNamedPipe(TEXT("\\\\.\\pipe\\EQTPipe"),
                            PIPE_ACCESS_DUPLEX | PIPE_TYPE_BYTE | PIPE_READMODE_BYTE,
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
				int i = 0;
				for (; i < strlen(buffer); i++)
				{
					if (isalnum(buffer[i]) == false || buffer[i] == ' ')
						break;
					text[i] = buffer[i];
				}
				EQTFunctions(text, sizeof(buffer));
            }
        }
		//FlushFileBuffers(hPipe);
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