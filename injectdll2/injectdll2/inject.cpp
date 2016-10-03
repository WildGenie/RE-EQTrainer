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
#include "Titanium\eqgame.h"
#include "Titanium\EQData.h"

#ifdef EQLIB_EXPORTS
#pragma message("EQLIB_EXPORTS")
#else
#pragma message("EQLIB_IMPORTS")
#endif

#ifdef EQLIB_EXPORTS
#define EQLIB_API extern "C" __declspec(dllexport)
#define EQLIB_VAR extern "C" __declspec(dllexport)
#define EQLIB_OBJECT __declspec(dllexport)
#else
#define EQLIB_API extern "C" __declspec(dllimport)
#define EQLIB_VAR extern "C" __declspec(dllimport)
#define EQLIB_OBJECT __declspec(dllimport)
#endif

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

typedef struct _SPAWNINFO {
	/* 0x0000 */   BYTE    Unknown0x0;
	/* 0x0001 */   CHAR    Lastname[0x27];  // // Surname on PCs, Newbie Tag on NPCs 
	/* 0x0028 */   DWORD     Unknown0x028[0x2];
	/* 0x0030 */   FLOAT     Y;
	/* 0x0034 */   FLOAT     X;
	/* 0x0038 */   FLOAT     Z;
	/* 0x003c */   FLOAT     SpeedY;
	/* 0x0040 */   FLOAT     SpeedX;
	/* 0x0044 */   FLOAT     SpeedZ;
	/* 0x0048 */   FLOAT     SpeedRun;
	/* 0x004c */   FLOAT     Heading;
	/* 0x0050 */   FLOAT     field_50;
	/* 0x0054 */   DWORD     field_54;
	/* 0x0058 */   DWORD     field_58;
	/* 0x005c */   FLOAT     CameraAngle;
	/* 0x0060 */   BYTE      Unknown0x60[0x50];
	/* 0x00b0 */   FLOAT     Y2;
	/* 0x00b4 */   FLOAT     X2;
	/* 0x00b8 */   FLOAT     Z2;
	/* 0x00bc */   FLOAT     SpeedY2;
	/* 0x00c0 */   FLOAT     SpeedX2;
	/* 0x00c4 */   FLOAT     SpeedZ2;
	/* 0x00c8 */   FLOAT     SpeedRun2;
	/* 0x00cc */   FLOAT     Heading2;
	/* 0x00d0 */   BYTE      Unknown0x0d0[0x50];
	/* 0x0120 */   CHAR      Name[0x40]; // ie priest_of_discord00 
	/* 0x0160 */   CHAR      DisplayedName[0x40]; // ie Priest of Discord 
	/* 0x01a0 */   FLOAT     SpeedHeading;
	/* 0x01a4 */   DWORD     field_1a4;
	/* 0x01a8 */   struct    _ACTORINFO   *pActorInfo;
	/* 0x01ac */   DWORD     field_1ac;
	/* 0x01b0 */   BYTE      CanFindLocation;
	/* 0x01b1 */   BYTE      Sneak;  // returns 01 on both Sneak and Shroud of Stealth 
	/* 0x01b2 */   BYTE      Linkdead;  // Uncharmable flag when used on mobs? 
	/* 0x01b3 */   BYTE      field_1b3;
	/* 0x01b4 */   BYTE      LFG;
	/* 0x01b5 */   BYTE      field_1b5;
	/* 0x01b6 */   BYTE      IsABoat; // 1 = a type of boat 
	/* 0x01b7 */   BYTE      Unknown0x1b7;
	/* 0x01b8 */   ARGBCOLOR ArmorColor[0x9];  // Armor Dye if custom, otherwise Item Tint 
	/* 0x01dc */   struct    _EQUIPMENT Equipment;
	/* 0x0200 */   DWORD     field_200;
	/* 0x0204 */   WORD      Zone;
	/* 0x0206 */   WORD      Instance;
	/* 0x0208 */   DWORD     field_208;
	/* 0x020c */   DWORD     field_20c;
	/* 0x0210 */   DWORD     field_210;
	/* 0x0214 */   struct    _SPAWNINFO *pNext;
	/* 0x0218 */   struct    _CHARINFO  *pCharInfo;
	/* 0x021c */   DWORD     field_218;
	/* 0x0220 */   struct    _SPAWNINFO *pPrev;
	/* 0x0224 */   BYTE      Unknown0x220[0x4];
	/* 0x0228 */   FLOAT     field_228;
	/* 0x022c */   DWORD     field_22c;
	/* 0x0230 */   FLOAT     RunSpeed;
	/* 0x0234 */   FLOAT     field_234;
	/* 0x0238 */   FLOAT     field_238;
	/* 0x023c */   FLOAT     AvatarHeight;  // height of avatar from ground when standing 
	/* 0x0240 */   FLOAT     WalkSpeed;
	/* 0x0244 */   BYTE      Type;
	/* 0x0245 */   BYTE      HairColor;
	/* 0x0246 */   BYTE      BeardColor;
	/* 0x0247 */   BYTE      Field_247;
	/* 0x0248 */   BYTE      Eyes;
	/* 0x0249 */   BYTE      Eyes2;
	/* 0x024a */   BYTE      BeardType;
	/* 0x024b */   BYTE      Holding;   // 0=holding/wielding nothing 
	/* 0x024c */   BYTE      Level;
	/* 0x024d */   BYTE      FaceHair;  // Face/Hair combination with headgear 
	/* 0x024e */   BYTE      Gender;
	/* 0x024f */   BYTE      PvPFlag;
	/* 0x0250 */   BYTE      HideMode;
	/* 0x0251 */   BYTE      StandState;
	/* 0x0252 */   BYTE      Class;
	/* 0x0253 */   BYTE      Light;
	/* 0x0254 */   BYTE      InNonPCRaceIllusion;  // This is buggy ...not sure exact usage 
	/* 0x0255 */   BYTE      Field_251;  // form related, unsure how 
	/* 0x0256 */   BYTE      GM;
	/* 0x0257 */   BYTE      Field_253;
	/* 0x0258 */   DWORD     SpawnID;
	/* 0x025c */   DWORD     MasterID;
	/* 0x0260 */   DWORD     Race;
	/* 0x0264 */   DWORD     Anon;
	/* 0x0268 */   DWORD     field_264;
	/* 0x026c */   DWORD     AFK;
	/* 0x0270 */   DWORD     BodyType;
	/* 0x0274 */   LONG      HPCurrent;
	/* 0x0278 */   BYTE      AARank;
	/* 0x0279 */   BYTE      Unknown0x278[0x3];
	/* 0x027c */   DWORD     GuildStatus;
	/* 0x0280 */   DWORD     Deity;
	/* 0x0284 */   DWORD     HPMax;
	/* 0x0288 */   DWORD     GuildID;
	/* 0x028c */   BYTE      Levitate;   //0-normal state  2=levitating 3=mob (not levitating) 
	/* 0x028d */   BYTE      Unknown0x28c[0x17];
	/* 0x02a4 */   CHAR      Title[0x20];
	/* 0x02c4 */   CHAR      Suffix[0x20];
	/* 0x02e4 */   CHAR      Unknown0x2e4[0x348 - 0x2e4];
	/*	        More Data */
} SPAWNINFO, *PSPAWNINFO;

class EQPlayer
{
public:
	EQLIB_OBJECT EQPlayer::~EQPlayer(void);
	EQLIB_OBJECT EQPlayer::EQPlayer(class EQPlayer *, unsigned char, unsigned int, unsigned char, char *);
	//SPAWNINFO Data;
};

EQPlayer **ppTarget = (EQPlayer**)pinstTarget;
#define pTarget (*ppTarget)

typedef VOID(__cdecl *fEQSendMessage)(PVOID, DWORD, PVOID, DWORD, BOOL);
fEQSendMessage    send_message = (fEQSendMessage)__SendMessage;
PVOID EQADDR_GWORLD = (PVOID)__gWorld;
VOID SendEQMessage(DWORD PacketType, PVOID pData, DWORD Length)
{
	if (!send_message || !EQADDR_GWORLD)
	{
		return;
	}
	send_message(EQADDR_GWORLD, PacketType, pData, Length, TRUE);
}

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