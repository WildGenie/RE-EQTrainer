#define OP_ExploreUnknown 0x0292		//used for unknown explorer
//world packets
#define OP_ApproveWorld 0x0710
#define OP_LogServer 0xc341
#define OP_MOTD 0xdd41
#define OP_SendLoginInfo 0x5818
#define OP_DeleteCharacter 0x5a40
#define OP_SendCharInfo 0x4740
#define OP_ExpansionInfo 0xd841
#define OP_CharacterCreate 0x4940
#define OP_ApproveName 0x8B40
#define OP_EnterWorld 0x0180
#define OP_SetChatServer 0x0980
#define OP_ZoneServerInfo 0x0480
#define OP_ZoneUnavail 0x0580
#define OP_WorldLogout 0x2340
#define OP_ChecksumExe 0x3941
#define OP_ChecksumSpell 0x3541

//Zone in opcodes
#define OP_ZoneEntry 0x2840
#define OP_ZoneEntryResend 0x4141
#define OP_ZoneInAvatarSet 0x6f40
#define OP_NewZone 0x5b40
#define OP_ReqClientSpawn 0x0a40
#define OP_ZoneSpawns 0x5f41
#define OP_CharInventory 0xf641
#define OP_SetServerFilter 0xff41
#define OP_SpawnDoor 0xf741
#define OP_ReqNewZone 0x5d40
#define OP_PlayerProfile 0x3640
#define OP_TimeOfDay 0xf240
#define OP_DataRate 0xe841


#define OP_Logout 0x5041
#define OP_LogoutReply 0x5941
#define OP_LevelUpdate 0x9841
#define OP_Stamina 0x5741

//Petition Opcodes
#define OP_Petition 0x0e40
#define OP_PetitionCheckout 0x8e41
#define OP_PetitionCheckIn 0x09e41
#define OP_PetitionDelete 0xa041
#define OP_DeletePetition 0x6422
#define OP_PetitionRefresh 0x1140
#define OP_PetitionViewPetition 0x6142

//Guild Opcodes
#define OP_GuildsList 0x9241
#define OP_GuildRemove 0x1941
#define OP_GuildPeace 0x9141
#define OP_GuildWar 0x6f41
#define OP_GuildLeader 0x9541
#define OP_GuildMOTD 0x0442
#define OP_SetGuildMOTD 0x0342
#define OP_GetGuildsList 0x2841
#define OP_GuildInvite 0x1741
#define OP_GuildDelete 0x1a41
#define OP_GuildInviteAccept 0x1841
#define OP_GuildAdded 0x7b41

//GM / guide opcodes
#define OP_GMServers 0xa840		// / servers
#define OP_GMZoneRequest 0x4f41		// / zone
#define OP_GMHideMe 0xd441		// / hideme
#define OP_GMGoto 0x6e40		// / goto
#define OP_GMDelCorpse 0xe941		// / delcorpse
#define OP_GMToggle 0xde41		// / toggle
#define OP_GMZoneRequest2 0x0842
#define OP_GMSummon 0xc540		// / summon
#define OP_GMEmoteZone 0xe341		// / emotezone
#define OP_GMFind 0x6940		// / find
#define OP_GMKick 0x6d40		// / kick
#define OP_GMNameChange 0xcb40
#define OP_GMBecomeNPC 0x8c41
#define OP_GMSearchCorpse 0xa741

//Loot
#define OP_LootItem 0xA040
#define OP_EndLootRequest 0x4F40
#define OP_LootComplete 0x4541
#define OP_LootRequest 0x4e40
#define OP_MoneyOnCorpse 0x5040

//AA
#define OP_RespondAA 0x1542
#define OP_AAAction 0x1442
#define OP_AAExpUpdate 0x2342

#define OP_SafePoint 0x2440
#define OP_Bind_Wound 0x9340
#define OP_GMTraining 0x9c40
#define OP_GMEndTraining 0x9d40
#define OP_GMTrainSkill 0x4041
#define OP_GMEndTrainingResponse 0x4341
#define OP_Animation 0xa140
#define OP_Taunt 0x3b41
#define OP_Stun 0x5b41
#define OP_MoneyUpdate 0x0840
#define OP_SendExpZonein 0xd840
#define OP_ReadBook 0xce40
#define OP_CombatAbility 0x6041
#define OP_Consume 0x5641
#define OP_Begging 0x2541
#define OP_InspectRequest 0xb540
#define OP_BeginCast 0xa940
#define OP_WhoAllRequest 0xf440
#define OP_WhoAllResponse 0x0b20
#define OP_Consent 0xb740
#define OP_LFGCommand 0xf041				//#define OP_LFG
#define OP_Bug 0xb340
#define OP_BoardBoat 0xbb41
#define OP_LeaveBoat 0xbc41
#define OP_Save 0x2e40
#define OP_Camp 0x0742
#define OP_AutoAttack 0x5141
#define OP_Consider 0x3741
#define OP_PetCommands 0x4542
#define OP_SpawnAppearance 0xf540
#define OP_DeleteSpawn 0x2940
#define OP_AutoAttack2 0x6141
#define OP_SetRunMode 0x1f40
#define OP_SaveOnZoneReq 0x5541
#define OP_MoveDoor 0x8e40
#define OP_SenseHeading 0x8741
#define OP_Buff 0x3241
#define OP_Key 0x5d42
#define OP_Split 0x3141
#define OP_Surname 0xc441
#define OP_MoveItem 0x2c41
#define OP_FaceChange 0x2542
#define OP_ItemPacket 0x6441
#define OP_ItemLinkResponse 0x6442
#define OP_ZoneChange 0xa340
#define OP_MemorizeSpell 0x8241
#define OP_SwapSpell 0xce41
#define OP_Forage 0x9440
#define OP_ConsentResponse 0xd540
#define OP_NewSpawn 0x6b42
#define OP_WearChange 0x9240
#define OP_OldSpecialMesg 0x8041
#define OP_SpecialMesg 0x8041
#define OP_Weather 0x3641
#define OP_Weather2 0x8a41
#define OP_Illusion 0x9140
#define OP_TargetMouse 0x6241
#define OP_InspectAnswer 0xb640
#define OP_GMKill 0x6c40
#define OP_ClickDoor 0x8d40
#define OP_YellForHelp 0xda41
#define OP_ManaChange 0x7f41
#define OP_ManaUpdate 0x1942
#define OP_ConsiderCorpse 0x3442
#define OP_CorpseDrag 0x1441
#define OP_SkillUpdate 0x8941
#define OP_CastSpell 0x7e41
#define OP_ClientUpdate 0xf340
#define OP_MobUpdate 0x9f40
#define OP_Report 0x5a42
#define OP_GroundSpawn 0x2c40
#define OP_TargetCommand 0xfe41
#define OP_Jump 0x2040
#define OP_ExpUpdate 0x9941
#define OP_Death 0x4a40
#define OP_GMLastName 0x6e41
#define OP_Mend 0x9d41
#define OP_TGB 0x2042
#define OP_InterruptCast 0x3542
#define OP_Action 0x4640
#define OP_Damage 0x5840
#define OP_Action2 0x1e40
#define OP_ChannelMessage 0x0741
#define OP_MultiLineMsg 0x1440
#define OP_Charm 0x4442
#define OP_DeleteSpell 0x4a42
#define OP_Assist 0x0042
#define OP_ClientError 0x4841
#define OP_DeleteCharge 0x4741
#define OP_ControlBoat 0x2641
#define OP_FeignDeath 0xac40
#define OP_Fishing 0x8f41
#define OP_InstillDoubt 0x9c41
#define OP_MoveCoin 0x2d41
#define OP_Translocate 0x0642
#define OP_Sacrifice 0xea41
#define OP_ApplyPoison 0xba41
#define OP_SendZonepoints 0xb440
#define OP_UpdateDoor 0x9840
#define OP_DespawnDoor 0x9b40
#define OP_Medding 0x5841
#define OP_Emote 0x1540
#define OP_EmoteText 0x1540
#define OP_RandomReq 0xe741
#define OP_RandomReply 0x1640
#define OP_Track 0x8441
#define OP_FormattedMessage 0x3642
#define OP_RequestClientZoneChange 0x4d41
#define OP_FriendsWho 0xc541
#define OP_Shielding 0x4b42
#define OP_Feedback 0x3c41

//bazaar trader stuff stuff :
//become and buy from
#define OP_BecomeTrader 0x1842
#define OP_TraderShop 0x1642
#define OP_Trader 0x1242
#define OP_TraderBuy 0x2442
#define OP_BazaarSearch 0x1142

//pc / npc trading
#define OP_TradeRequest 0xd140
#define OP_TradeAcceptClick 0xda40
#define OP_TradeRequestAck 0xe640
#define OP_TradeCoins 0xe440
#define OP_FinishTrade 0xdc40
#define OP_CancelTrade 0xdb40
#define OP_TradeMoneyUpdate 0x3d41
#define OP_TradeReset 0x1040
#define OP_TradeRefused 0xd640

//merchant crap
#define OP_ShopPlayerSell 0x2740
#define OP_ShopEnd 0x3740
#define OP_ShopEndConfirm 0x4641
#define OP_ShopPlayerBuy 0x3540
#define OP_ShopRequest 0x0b40
#define OP_ShopDelItem 0x3840
#define OP_ShopInventoryPacket 0x0c40

//tradeskill stuff :
#define OP_ClickObject 0x2b40
#define OP_ClickObjectAction 0xd740
#define OP_TradeSkillCombine 0x0541

#define OP_RequestDuel 0xcf40
#define OP_DuelResponse 0xd040
#define OP_DuelResponse2 0x5d41

#define OP_RezzComplete 0xec41
#define OP_RezzRequest 0x2a41
#define OP_RezzAnswer 0x9b41
#define OP_SafeFallSuccess 0xab41

//Group Opcodes
#define OP_GroupDisband 0x4440
#define OP_GroupInvite 0x3e20
#define OP_GroupFollow 0x3d20
#define OP_GroupUpdate 0x2620
#define OP_GroupInvite2 0x4040		//this is sometimes sent instead of #define OP_GroupInvite
#define OP_GroupCancelInvite 0x4140

#define OP_RaidInvite 0x5f42
#define OP_RaidUpdate 0x6042

#define OP_ClearObject 0x0542
#define OP_Discipline 0xe641
#define OP_DisciplineChange 0xf241
#define OP_Sound 0x8040

//Rogue packets
#define OP_SenseTraps 0x8841
#define OP_PickPocket 0xad40
#define OP_DisarmTraps 0xf341
#define OP_Disarm 0xaa40
#define OP_Hide 0x8641
#define OP_Sneak 0x8541

//Mac item Opcodes
#define OP_SummonedItem 0x7841
#define OP_ContainerPacket 0x6641;
#define OP_BookPacket 0x6541;
#define OP_TradeItemPacket 0xdf40;
#define OP_LootItemPacket 0x5240;
#define OP_MerchantItemPacket 0x3140;
#define OP_ObjectItemPacket 0xfb40;

#define OP_SetTitle 0xd440
#define OP_Projectile 0x4540

#define OP_SoulMarkAdd 0xD241
#define OP_SoulMarkList 0xD141
#define OP_SoulMarkUpdate 0xD041

//Message Boards in PC Client
// #define OP_MBRetrievalRequest 0x0841
// #define OP_MBRetrievalDetailRequest 0x0941
// #define OP_MBRetrievalResponse 0x0a41
// #define OP_MBRetrievalDetailResponse 0x0b41
// #define OP_MBRetrievalPostRequest 0x0c41
// #define OP_MBRetrievalEraseRequest 0x0d41
// #define OP_MBRetrievalFin 0x0e41

//Login opcodes
#define OP_SessionReady 0x0001
#define OP_Login 0x0002
#define OP_ServerListRequest 0x0004
#define OP_PlayEverquestRequest 0x000d
#define OP_PlayEverquestResponse 0x0021
#define OP_ChatMessage 0x0016
#define OP_LoginAccepted 0x0017
#define OP_ServerListResponse 0x0018
#define OP_Poll 0x0029
#define OP_EnterChat 0x000f
#define OP_PollResponse 0x0011


#define OP_MobHealth 0xb240
#define OP_HPUpdate 0xb240


#define MoveLocalPlayerToSafeCoords 0x004B459C

struct TradeRequest_Struct {
	/*00*/	uint16_t to_mob_id;
	/*04*/	uint16_t from_mob_id;
	/*08*/
};

typedef struct _MovePkt {
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
} MovePkt;