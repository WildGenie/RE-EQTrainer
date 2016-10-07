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
#include "Titanium\opcodes.h"
//#include "EQMac\opcodes.h"

using namespace std;

// Movement function
VOID MoveTo(float x, float y, float z) {
	PSPAWNINFO pMe = GetCharInfo()->pSpawn;

	MovePkt mp;
	ZeroMemory(&mp, sizeof(mp));

	mp.SpawnID = pMe->SpawnID;
	mp.Heading = pMe->Heading;
	mp.X = x;
	mp.Y = y;
	mp.Z = z;

	SendEQMessage(OP_ClientUpdate, &mp, sizeof(mp));
}

VOID EQTFunctions (const char *func, int len) {
	char newText[1024] = { 0 };
	strncpy(newText, func, len);

	// DEBUG
	//wchar_t *text = new wchar_t[len];
	//mbstowcs(text, newText, len);
	//MessageBox(NULL, text, NULL, MB_OK);

	char cmd[1024] = { 0 };
	strcpy(cmd, newText);
	strtok(cmd, " ");

	if(strcmp("warp", cmd) == 0){
		typedef void (__thiscall* CGCamera__ResetView)();
		CGCamera__ResetView ResetView = (CGCamera__ResetView)MoveLocalPlayerToSafeCoords; //TITANIUM = 0x0043D7C5   //MAC = 0x004B459C  //UNDERFOOT = 0x00499CE8?
		ResetView();
	}
	if (strcmp("opentrade", cmd) == 0) {
		PSPAWNINFO pMe = GetCharInfo()->pSpawn;
		if (!pMe) {
			MessageBox(NULL, L"ERROR: Cant retrieve our spawn ID", NULL, MB_OK);
			return;
		}

		PSPAWNINFO pMyTarget = (PSPAWNINFO)pTarget;
		if (!pTarget || !ppTarget) {
			MessageBox(NULL, L"ERROR: No target selected", NULL, MB_OK);
			return;
		}

		TradeRequest_Struct trStruct;
		trStruct.to_mob_id = pMyTarget->SpawnID;
		trStruct.from_mob_id = pMe->SpawnID;

		SendEQMessage(OP_TradeRequest, &trStruct, sizeof(trStruct));
		SendEQMessage(OP_TradeRequestAck, &trStruct, sizeof(trStruct));
	}
	if (strcmp("acceptgive", cmd) == 0) {
		SendWndClick("GiveWnd", "GVW_Give_Button", "leftmouseup");
	}
	if (strcmp("useskill", cmd) == 0) {
		//http://wiki.eqemulator.org/p?Skills

		PSPAWNINFO pMyTarget = (PSPAWNINFO)pTarget;
		if (!pTarget || !ppTarget) {
			MessageBox(NULL, L"ERROR: No target selected", NULL, MB_OK);
			return;
		}

		MoveTo(pMyTarget->X, pMyTarget->Y, pMyTarget->Z);

		const char *arg = newText + 9;
		pCharData1->UseSkill((unsigned char)arg, (EQPlayer*)pCharData1);

		MoveTo(pMyTarget->X, pMyTarget->Y, pMyTarget->Z);
	}
	if (strcmp("summoncorpse", cmd) == 0) {

		PSPAWNINFO pMe = GetCharInfo()->pSpawn;
		if (!pMe) {
			MessageBox(NULL, L"ERROR: Cant retrieve our spawn ID", NULL, MB_OK);
			return;
		}

		// check for target
		if (!pTarget || !ppTarget) return;

		// make sure it's a corpse
		PSPAWNINFO Target = (PSPAWNINFO)pTarget;
		if (Target->Type != SPAWN_CORPSE) return;

		// setup move packet
		struct _MOVEPKT {
			/*0000*/ unsigned short SpawnID;
			/*0002*/ unsigned short TimeStamp;
			/*0004*/ float Y;
			/*0008*/ float DeltaZ;
			/*0012*/ float DeltaY;
			/*0016*/ float DeltaX;
			/*0020*/ int Animation : 10;
			/*0020*/ int DeltaHeading : 10;
			/*0020*/ int padding0020 : 12;
			/*0024*/ float X;
			/*0028*/ float Z;
			/*0032*/ int Heading : 12;
			/*0032*/ int padding1_0032 : 10;
			/*0032*/ int padding2_0032 : 10;
		} P; // 36

			 // init move packet
		ZeroMemory(&P, sizeof(P));
		P.SpawnID = (unsigned short)pMe->SpawnID;
		P.Heading = (unsigned int)(pMe->Heading * 4);

		// jump to
		P.Z = Target->Z;
		P.Y = Target->Y;
		P.X = Target->X;
		SendEQMessage(PKT_UPDATE_POSITION, &P, sizeof(P));

		// corpse drag
		char szCorpseName[152] = { 0 };
		strcpy(szCorpseName, Target->Name);
		SendEQMessage(PKT_CORPSE_DRAG, szCorpseName, 152);

		// jump back
		P.Z = pMe->Z;
		P.Y = pMe->Y;
		P.X = pMe->X;
		SendEQMessage(PKT_UPDATE_POSITION, &P, sizeof(P));

		// corpse drop
		SendEQMessage(PKT_CORPSE_DROP, "", 0);
	}
	if (strcmp("saytarget", cmd) == 0) {
		const char *arg = newText + 10; //our message

		PSPAWNINFO pMe = GetCharInfo()->pSpawn;
		if (!pMe) {
			MessageBox(NULL, L"ERROR: Cant retrieve our spawn ID", NULL, MB_OK);
			return;
		}

		PSPAWNINFO pMyTarget = (PSPAWNINFO)pTarget;
		if (!pTarget || !ppTarget) {
			MessageBox(NULL, L"ERROR: No target selected", NULL, MB_OK);
			return;
		}

		CHAR SendMsg[MAX_STRING] = { 0 };

		MoveTo(pMyTarget->X, pMyTarget->Y, pMyTarget->Z);

		sprintf(SendMsg, "/say %s", arg);
		DoCommand(pMe, SendMsg);

		//DEBUG
		//CHAR dbg[MAX_STRING] = { 0 };
		//sprintf(dbg, "cmd:%s x:%.2f y:%.2f z:%.2f", SendMsg, pMyTarget->X, pMyTarget->Y, pMyTarget->Z);
		//wchar_t *text = new wchar_t[sizeof(dbg)];
		//mbstowcs(text, dbg, sizeof(dbg));
		//MessageBox(NULL, text, NULL, MB_OK);

		MoveTo(pMyTarget->X, pMyTarget->Y, pMyTarget->Z);
	}
	memset(cmd, 0, 1024);
	memset(newText, 0, 1024);
	//delete text; //if we debug
	return;
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
					if (isprint(buffer[i]) == false)
						break;

					text[i] = buffer[i];
				}
				EQTFunctions(text, sizeof(buffer));
				memset(text, 0, 1024);
				memset(buffer, 0, 1024);
				dwRead = {};
            }
        }
		FlushFileBuffers(hPipe);
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