

#define OP_Unknown 0x0000
#define OP_ExploreUnknown 0x0000

//world packets
#define OP_ApproveWorld 0x3c25			
#define OP_LogServer 0x0fa6			
#define OP_MOTD 0x024d			
#define OP_SendLoginInfo 0x4dd0			
#define OP_DeleteCharacter 0x26c9			
#define OP_SendCharInfo 0x4513			
#define OP_ExpansionInfo 0x04ec			
#define OP_CharacterCreate 0x10b2	
#define OP_RandomNameGenerator 0x23d4	
#define OP_GuildsList 0x6957	
#define OP_ApproveName 0x3ea6		
#define OP_EnterWorld 0x7cba			
#define OP_PostEnterWorld 0x52A4	
#define OP_World_Client_CRC1 0x5072			
#define OP_World_Client_CRC2 0x5b18			
#define OP_SetChatServer 0x00d7			
#define OP_SetChatServer2 0x6536			
#define OP_ZoneServerInfo 0x61b6			
#define OP_WorldComplete 0x509d			
#define OP_ZoneUnavail 0x407C
#define OP_WorldClientReady 0x5e99	
#define OP_WorldUnknown001 0x0000		
#define OP_CharacterStillInZone 0x60fa	
#define OP_WorldChecksumFailure 0x7D37	
#define OP_WorldLoginFailed 0x8DA7	
#define OP_WorldLogout 0x7718		
#define OP_WorldLevelTooHigh 0x583b		
#define OP_CharInacessable 0x436A		

//Zone in opcodes
#define OP_ZoneEntry 0x7213			
#define OP_ZoneInUnknown 0x0000
#define OP_AckPacket 0x7752			
#define OP_NewZone 0x0920			
#define OP_ReqClientSpawn 0x0322	
#define OP_ZoneSpawns 0x2e78			
#define OP_CharInventory 0x5394		
#define OP_SetServerFilter 0x6563			
#define OP_LockoutTimerInfo 0x7f63
#define OP_SendZonepoints 0x3eba			
#define OP_SpawnDoor 0x4c24			
#define OP_ReqNewZone 0x7ac5			
#define OP_PlayerProfile 0x75df			
#define OP_TimeOfDay 0x1580			
#define OP_ZoneServerReady 0x0000	


#define OP_Logout 0x61ff
#define OP_LogoutReply 0x3cdc	
#define OP_PreLogoutReply 0x711e		
#define OP_LevelUpdate 0x6d44
#define OP_Stamina 0x7a83			

//Petition Opcodes
#define OP_PetitionSearch 0x0000            
#define OP_PetitionSearchResults 0x0000     
#define OP_PetitionSearchText 0x0000   
#define OP_Petition 0x251f
#define OP_PetitionUpdate 0x0000	#guess
#define OP_PetitionCheckout 0x0000
#define OP_PetitionCheckIn 0x0000
#define OP_PetitionQue 0x33c3
#define OP_PetitionUnCheckout 0x0000
#define OP_PetitionDelete 0x5692
#define OP_DeletePetition 0x0000
#define OP_PetitionResolve 0x0000	#0x0000
#define OP_PDeletePetition 0x0000
#define OP_PetitionBug 0x0000
#define OP_PetitionRefresh 0x0000
#define OP_PetitionCheckout2 0x0000
#define OP_PetitionViewPetition 0x0000

//Guild Opcodes
//list to client : 0x0F4D, 0x147d, 0x18B7, 0x2ec9, 0x3230, 0x32CF, 0x461A, 0x4CC7
// 0x6966, 0x712A, 0x754E, 0x7C32, 0x7C59
// some from client : 0x7825
#define OP_ZoneGuildList 0x6957		
#define OP_GetGuildMOTD 0x7fec
#define OP_GetGuildMOTDReply 0x3246
#define OP_GuildMemberList 0x147d			
#define OP_GuildMemberUpdate 0x0f4d			
#define OP_GuildMemberLevelUpdate 0x0000		
#define OP_GuildRemove 0x0179
#define OP_GuildPeace 0x215a
#define OP_GuildWar 0x0c81
#define OP_GuildLeader 0x12b1
#define OP_GuildDemote 0x4eb9
#define OP_GuildMOTD 0x475a
#define OP_SetGuildMOTD 0x591c
#define OP_GetGuildsList 0x0000
#define OP_GuildInvite 0x18b7
#define OP_GuildPublicNote 0x17a2			
#define OP_GuildDelete 0x6cce
#define OP_GuildInviteAccept 0x61d0
#define OP_GuildManageRemove 0x0000
#define OP_GuildManageAdd 0x0000
#define OP_GuildManageStatus 0x0000
#define OP_GuildManageBanker 0x3d1e
#define OP_GuildBank 0xb4ab
#define OP_SetGuildRank 0x6966
#define OP_LFGuild 0x1858

//GM / guide opcodes
#define OP_GMServers 0x3387		
#define OP_GMBecomeNPC 0x7864		
#define OP_GMZoneRequest 0x1306	
#define OP_GMSearchCorpse 0x3c32	
#define OP_GMHideMe 0x15b2	
#define OP_GMGoto 0x1cee	
#define OP_GMDelCorpse 0x0b2f	
#define OP_GMApproval 0x0c0f	
#define OP_GMToggle 0x7fea	
#define OP_GMZoneRequest2 0x244c
#define OP_GMSummon 0x1edc	
#define OP_GMEmoteZone 0x39f2	
#define OP_GMEmoteWorld 0x3383	
#define OP_GMFind 0x5930		
#define OP_GMKick 0x692c	
#define OP_GMNameChange 0x0000

#define OP_SafePoint 0x0000
#define OP_Bind_Wound 0x601d
#define OP_GMTraining 0x238f
#define OP_GMEndTraining 0x613d
#define OP_GMTrainSkill 0x11d2
#define OP_GMEndTrainingResponse 0x0000
#define OP_Animation 0x2acf			
#define OP_Stun 0x1E51
#define OP_MoneyUpdate 0x267c
#define OP_SendExpZonein 0x0587			
#define OP_IncreaseStats 0x5b7b
#define OP_CrashDump 0x7825
#define OP_ReadBook 0x1496
#define OP_Dye 0x00dd			
#define OP_Consume 0x77d6			
#define OP_Begging 0x13e7			
#define OP_InspectRequest 0x775d			
#define OP_InspectAnswer 0x2403			
#define OP_Action2 0x0000
#define OP_BeginCast 0x3990			
#define OP_ColoredText 0x0b2d			
#define OP_Consent 0x1081			
#define OP_ConsentDeny 0x4e8c			
#define OP_ConsentResponse 0x6380			
#define OP_LFGCommand 0x68ac			
#define OP_LFGGetMatchesRequest 0x022f			
#define OP_LFGAppearance 0x1a85
#define OP_LFGResponse 0x0000
#define OP_LFGGetMatchesResponse 0x45d0			
#define OP_LootItem 0x7081			
#define OP_Bug 0x7ac2			
#define OP_BoardBoat 0x4298			
#define OP_Save 0x736b			
#define OP_Camp 0x78c1			
#define OP_EndLootRequest 0x2316			
#define OP_MemorizeSpell 0x308e			
#define OP_LinkedReuse 0x6a00
#define OP_SwapSpell 0x2126			
#define OP_CastSpell 0x304b			
#define OP_DeleteSpell 0x4f37
#define OP_LoadSpellSet 0x403e			
#define OP_AutoAttack 0x5e55			
#define OP_AutoFire 0x6c53
#define OP_Consider 0x65ca			
#define OP_Emote 0x547a			
#define OP_PetCommands 0x10a1			
#define OP_PetBuffWindow 0x4e31
#define OP_SpawnAppearance 0x7c32			
#define OP_DeleteSpawn 0x55bc			
#define OP_FormattedMessage 0x5a48			
#define OP_WhoAllRequest 0x5cdd			
#define OP_WhoAllResponse 0x757b			
#define OP_AutoAttack2 0x0701			
#define OP_SetRunMode 0x4aba			
#define OP_SimpleMessage 0x673c			
#define OP_SaveOnZoneReq 0x1540			
#define OP_SenseHeading 0x05ac			
#define OP_Buff 0x6a53			
#define OP_LootComplete 0x0a94			
#define OP_EnvDamage 0x31b3			
#define OP_Split 0x4848			
#define OP_Surname 0x4668			
#define OP_ClearSurname 0x6cdb
#define OP_MoveItem 0x420f			
#define OP_FaceChange 0x0f8e			
#define OP_ItemPacket 0x3397			
#define OP_ItemLinkResponse 0x667c			
#define OP_ClientReady 0x5e20			
#define OP_ZoneChange 0x5dd8			
#define OP_ItemLinkClick 0x53e5			
#define OP_Forage 0x4796
#define OP_BazaarSearch 0x1ee9			
#define OP_NewSpawn 0x1860			
//a similar unknonw packet to NewSpawn : 0x12b2
#define OP_WearChange 0x7441			
#define OP_Action 0x497c			
#define OP_SpecialMesg 0x2372			
#define OP_Bazaar 0x0000
#define OP_LeaveBoat 0x67c9			
#define OP_Weather 0x254d			
#define OP_LFPGetMatchesRequest 0x35a6			
#define OP_Illusion 0x448d			
#define OP_TargetReject 0x0000
#define OP_TargetCommand 0x1477
#define OP_TargetMouse 0x6c47			
#define OP_TargetHoTT 0x6a12
#define OP_GMKill 0x6980			
#define OP_MoneyOnCorpse 0x7fe4			
#define OP_ClickDoor 0x043b			
#define OP_MoveDoor 0x700d			
#define OP_RemoveAllDoors 0x77d0
#define OP_LootRequest 0x6f90			
#define OP_YellForHelp 0x61ef			
#define OP_ManaChange 0x4839			
#define OP_LFPCommand 0x6f82			
#define OP_RandomReply 0x6cd5			
#define OP_DenyResponse 0x7c66			
#define OP_ConsiderCorpse 0x773f			
#define OP_CorpseDrag 0x50c0			
#define OP_CorpseDrop 0x7c7c			
#define OP_ConfirmDelete 0x3838			
#define OP_MobHealth 0x0695			
#define OP_SkillUpdate 0x6a93			
#define OP_RandomReq 0x5534			
#define OP_ClientUpdate 0x14cb			
#define OP_Report 0x7f9d
#define OP_GroundSpawn 0x0f47			
#define OP_LFPGetMatchesResponse 0x06c5
#define OP_Jump 0x0797			
#define OP_ExpUpdate 0x5ecd			
#define OP_Death 0x6160			
#define OP_BecomeCorpse 0x4DBC
#define OP_GMLastName 0x23a1			
#define OP_InitialMobHealth 0x3d2d			
#define OP_Mend 0x14ef			
#define OP_MendHPUpdate 0x0000
#define OP_Feedback 0x5306			
#define OP_TGB 0x0c11			
#define OP_InterruptCast 0x0b97
#define OP_Damage 0x5c78			
#define OP_ChannelMessage 0x1004			
#define OP_LevelAppearance 0x358e
#define OP_MultiLineMsg 0x0000
#define OP_Charm 0x12e5
#define OP_ApproveZone 0x0000
#define OP_Assist 0x7709
#define OP_AssistGroup 0x5104
#define OP_AugmentItem 0x539b
#define OP_BazaarInspect 0x0000
#define OP_ClientError 0x0000
#define OP_DeleteItem 0x4d81
#define OP_DeleteCharge 0x1c4a
#define OP_ControlBoat 0x2c81
#define OP_DumpName 0x0000
#define OP_FeignDeath 0x7489
#define OP_Heartbeat 0x0000
#define OP_ItemName 0x0000
#define OP_LDoNButton 0x13c8
#define OP_MoveCoin 0x7657
#define OP_ReloadUI 0x0000
#define OP_ZonePlayerToBind 0x385e	
#define OP_Rewind 0x4cfa		
#define OP_Translocate 0x8258
#define OP_Sacrifice 0x727a
#define OP_KeyRing 0x68c4
#define OP_ApplyPoison 0x0c2c
#define OP_AugmentInfo 0x45ff		
#define OP_SetStartCity 0x41dc		
#define OP_SpellEffect 0x22C5
#define OP_RemoveNimbusEffect 0x0000
#define OP_CrystalReclaim 0x7cfe
#define OP_CrystalCreate 0x62c3
#define OP_Marquee 0x1d4d
#define OP_CancelSneakHide 0x48C2

#define OP_DzQuit 0x486d
#define OP_DzListTimers 0x39aa
#define OP_DzAddPlayer 0x7fba
#define OP_DzRemovePlayer 0x540b
#define OP_DzSwapPlayer 0x794a
#define OP_DzMakeLeader 0x0ce9
#define OP_DzPlayerList 0xada0
#define OP_DzJoinExpeditionConfirm 0x3817
#define OP_DzJoinExpeditionReply 0x5da9
#define OP_DzExpeditionInfo 0x98e
#define OP_DzMemberStatus 0x1826
#define OP_DzLeaderStatus 0x7abc
#define OP_DzExpeditionEndsWarning 0x1c3f
#define OP_DzExpeditionList 0x7c12
#define OP_DzMemberList 0x9b6
#define OP_DzCompass 0x28aa
#define OP_DzChooseZone 0x1022
//0x330d is something but I'm not sure what yet.

//bazaar trader stuff stuff :
//become and buy from
//Server->Client: [Opcode:#define OP_Unknown(0x0000) Size : 8]
//   0: 46 01 00 00 39 01 00 00 | F...9...
#define OP_TraderDelItem 0x0da9
#define OP_BecomeTrader 0x2844
#define OP_TraderShop 0x35e8
#define OP_TraderItemUpdate 0x0000
#define OP_Trader 0x524e
#define OP_ShopItem 0x0000
#define OP_TraderBuy 0x6dd8			
#define OP_Barter 0x7460

//pc / npc trading
#define OP_TradeRequest 0x372f
#define OP_TradeAcceptClick 0x0065
#define OP_TradeRequestAck 0x4048
#define OP_TradeCoins 0x34c1
#define OP_FinishTrade 0x6014
#define OP_CancelTrade 0x2dc1
#define OP_TradeBusy 0x6839
#define OP_TradeMoneyUpdate 0x0000

//merchant crap
#define OP_ShopPlayerSell 0x0e13		
#define OP_ShopEnd 0x7e03			
#define OP_ShopEndConfirm 0x20b2
#define OP_ShopPlayerBuy 0x221e
#define OP_ShopRequest 0x45f9		
#define OP_ShopDelItem 0x0da9

//tradeskill stuff :
//something 0x21ed (8)
//something post combine 0x5f4e (8)
#define OP_ClickObject 0x3bc2
#define OP_ClickObjectAction 0x6937
#define OP_ClearObject 0x21ed
//0x711e of len 0 comes right after #define OP_ClickObjectAck from server
#define OP_RecipeDetails 0x4ea2
#define OP_RecipesFavorite 0x23f0
#define OP_RecipesSearch 0x164d
#define OP_RecipeReply 0x31f8
#define OP_RecipeAutoCombine 0x0353
#define OP_TradeSkillCombine 0x0b40

#define OP_RequestDuel 0x28e1
#define OP_DuelResponse 0x3bad
#define OP_DuelResponse2 0x1b09	

#define OP_RezzComplete 0x4b05
#define OP_RezzRequest 0x1035
#define OP_RezzAnswer 0x6219
#define OP_SafeFallSuccess 0x3b21
#define OP_Shielding 0x3fe6
#define OP_TestBuff 0x6ab0	
#define OP_Track 0x5d11			
#define OP_TrackTarget 0x7085
#define OP_TrackUnknown 0x6177		

//Tribute Packets :
#define OP_OpenGuildTributeMaster 0x0000
#define OP_OpenTributeMaster 0x512e		
#define OP_OpenTributeReply 0x27B3	
#define OP_SelectTribute 0x625d	
#define OP_TributeItem 0x6f6c	
#define OP_TributeMoney 0x27b3		
#define OP_TributeNPC 0x7f25		
#define OP_TributeToggle 0x2688		
#define OP_TributeTimer 0x4665 
#define OP_TributePointUpdate 0x6463
#define OP_TributeUpdate 0x5639			
#define OP_GuildTributeInfo 0x5e3d			
#define OP_TributeInfo 0x152d
#define OP_SendGuildTributes 0x5e3a
#define OP_SendTributes 0x067a	
// 27b3 4665

//Adventure packets :
#define OP_LeaveAdventure 0x0c0d
#define OP_AdventureFinish 0x3906
#define OP_AdventureInfoRequest 0x2aaf	
#define OP_AdventureInfo 0x1db5			
#define OP_AdventureRequest 0x43fd
#define OP_AdventureDetails 0x3f26
#define OP_AdventureData 0x0677
#define OP_AdventureUpdate 0x64ac
#define OP_AdventureMerchantRequest 0x0950
#define OP_AdventureMerchantResponse 0x4416
#define OP_AdventureMerchantPurchase 0x413d
#define OP_AdventureMerchantSell 0x0097
#define OP_AdventurePointsUpdate 0x420a	
#define OP_AdventureStatsRequest 0x5fc7
#define OP_AdventureStatsReply 0x56cd
#define OP_AdventureLeaderboardRequest 0x230a
#define OP_AdventureLeaderboardReply 0x0d0f
// request stats : 0x5fc7, reply 0x56cd ?
// request leaderboard : 0x230a ? , reply 0x0d0f ?

//Group Opcodes
#define OP_GroupDisband 0x0e76			
#define OP_GroupInvite 0x1b48			
#define OP_GroupFollow 0x7bc7			
#define OP_GroupUpdate 0x2dd6			
#define OP_GroupAcknowledge 0x0000
#define OP_GroupCancelInvite 0x1f27			
#define OP_GroupDelete 0x0000
#define OP_GroupFollow2 0x0000 
#define OP_GroupInvite2 0x12d6	
#define OP_CancelInvite 0x0000

#define OP_RaidJoin 0x1f21			
#define OP_RaidInvite 0x5891			
#define OP_RaidUpdate 0x1f21		

#define OP_InspectBuffs 0x4FB6


#define OP_ZoneComplete 0x0000
#define OP_ItemLinkText 0x0000
#define OP_DisciplineUpdate 0x7180
#define OP_DisciplineTimer 0x53df
#define OP_LocInfo 0x0000
#define OP_FindPersonRequest 0x3c41			
#define OP_FindPersonReply 0x5711			
#define OP_ForceFindPerson 0x0000
#define OP_LoginComplete 0x0000
#define OP_Sound 0x541e
#define OP_Zone_MissingName01 0x0000	
#define OP_MobRename 0x0498			
#define OP_BankerChange 0x6a5b

//Button - push commands
#define OP_Taunt 0x5e48
#define OP_CombatAbility 0x5ee8
#define OP_SenseTraps 0x5666			
#define OP_PickPocket 0x2ad8
#define OP_DisarmTraps 0x1241
#define OP_Disarm 0x17d9
#define OP_Hide 0x4312
#define OP_Sneak 0x74e1
#define OP_Fishing 0x0b36
#define OP_InstillDoubt 0x389e	
#define OP_LDoNOpen 0x083b

//Task packets
#define OP_TaskActivityComplete 0x54eb
#define OP_CompletedTasks 0x76a2			
#define OP_TaskDescription 0x5ef7			
#define OP_TaskActivity 0x682d			
#define OP_TaskMemberList 0x722f	#not sure
#define OP_OpenNewTasksWindow 0x5e7c	
#define OP_AvaliableTask 0x0000
#define OP_AcceptNewTask 0x207f
#define OP_TaskHistoryRequest 0x5df4
#define OP_TaskHistoryReply 0x397d
#define OP_CancelTask 0x3ba8
#define OP_DeclineAllTasks 0x0000	
#define OP_TaskMemberInvite 0x79b4
#define OP_TaskMemberInviteResponse 0x0358
#define OP_TaskMemberChange 0x5886
#define OP_TaskMakeLeader 0x1b25
#define OP_TaskAddPlayer 0x6bc4
#define OP_TaskRemovePlayer 0x37b9
#define OP_TaskPlayerList 0x3961
#define OP_TaskQuit 0x35dd
//task complete related : 0x0000 (24 bytes), 0x0000 (8 bytes), 0x0000 (4 bytes)


#define OP_RequestClientZoneChange 0x7834			

#define OP_SendAATable 0x367d			
#define OP_UpdateAA 0x5966
#define OP_RespondAA 0x3af4
#define OP_SendAAStats 0x5996			
#define OP_AAAction 0x0681			
#define OP_AAExpUpdate 0x5f58			

#define OP_PurchaseLeadershipAA 0x17bf
#define OP_UpdateLeadershipAA 0x07f1
#define OP_LeadershipExpUpdate 0x596e
#define OP_LeadershipExpToggle 0x5b37
#define OP_MarkNPC 0x5483
#define OP_ClearNPCMarks 0x3ef6
#define OP_DoGroupLeadershipAbility 0x569e
#define OP_DelegateAbility 0x10f4
#define OP_SetGroupTarget 0x3eec

//The following 4 Opcodes are for SoF only :
#define OP_FinishWindow 0x0000	
#define OP_FinishWindow2 0x0000		
#define OP_ItemVerifyRequest 0x0000	
#define OP_ItemVerifyReply 0x0000	

//discovered opcodes not yet used :
#define OP_PlayMP3 0x26ab
#define OP_FriendsWho 0x48fe
#define OP_MoveLogRequest 0x7510		
#define OP_MoveLogDisregard 0x0000		
#define OP_ReclaimCrystals 0x7cfe
#define OP_CrystalCountUpdate 0x0ce3
#define OP_DynamicWall 0x0000
#define OP_CustomTitles 0x2a28			
#define OP_NewTitlesAvailable 0x4eca		
#define OP_RequestTitles 0x5eba			
#define OP_SendTitleList 0x3e89		
#define OP_SetTitle 0x1f22			
#define OP_SetTitleReply 0x5eab			
#define OP_Bandolier 0x6f0c
#define OP_PotionBelt 0x0719
#define OP_OpenDiscordMerchant 0x0000	
#define OP_DiscordMerchantInventory 0x0000	
#define OP_GiveMoney 0x0000	
#define OP_OnLevelMessage 0x1dde
#define OP_PopupResponse 0x3816
#define OP_RequestKnowledgeBase 0x0000
#define OP_KnowledgeBase 0x0000
#define OP_SlashAdventure 0x571a	
#define OP_VetRewardsAvaliable 0x0557
#define OP_VetClaimRequest 0x6ba0
#define OP_VetClaimReply 0x407e
#define OP_BecomePVPPrompt 0x36B2
#define OP_PVPStats 0x5cc0
#define OP_PVPLeaderBoardRequest 0x61d2
#define OP_PVPLeaderBoardReply 0x1a59
#define OP_PVPLeaderBoardDetailsRequest 0x06a2
#define OP_PVPLeaderBoardDetailsReply 0x246a
#define OP_PickLockSuccess 0x40E7
#define OP_WeaponEquip1 0x6c5e
#define OP_PlayerStateAdd 0x63da
#define OP_PlayerStateRemove 0x381d
#define OP_VoiceMacroIn 0x2866		
#define OP_VoiceMacroOut 0x2ec6		
#define OP_CameraEffect 0x0937		

//named unknowns, to make looking for real unknown easier
#define OP_AnnoyingZoneUnknown 0x729c
#define OP_Some6ByteHPUpdate 0x0000		
#define OP_SomeItemPacketMaybe 0x4033	
#define OP_QueryResponseThing 0x1974
#define OP_FloatListThing 0x6a1b	

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

//raw opcodes
#define OP_RAWSessionRequest 0x0000
#define OP_RAWSessionResponse 0x0000
#define OP_RAWCombined 0x0000
#define OP_RAWSessionDisconnect 0x0000
#define OP_RAWKeepAlive 0x0000
#define OP_RAWSessionStatRequest 0x0000
#define OP_RAWSessionStatResponse 0x0000
#define OP_RAWPacket 0x0000
#define OP_RAWFragment 0x0000
#define OP_RAWOutOfOrderAck 0x0000
#define OP_RAWAck 0x0000
#define OP_RAWAppCombined 0x0000
#define OP_RAWOutOfSession 0x0000

//mail opcodes
#define OP_Command 0x0000
#define OP_MailboxHeader 0x0000
#define OP_MailHeader 0x0000
#define OP_MailBody 0x0000
#define OP_NewMail 0x0000
#define OP_SentConfirm 0x0000


#define OP_MobUpdate 0x0000	

//we need to document the differences between these packets to make identifying them easier
#define OP_MobHealth 0x0695
#define OP_HPUpdate 0x3bcf			
#define OP_Some3ByteHPUpdate 0x0000
#define OP_InitialHPUpdate 0x0000

#define MoveLocalPlayerToSafeCoords 0x43D7C5
#define PKT_CORPSE_DRAG				0x50C0
#define PKT_CORPSE_DROP				0x7C7C
#define PKT_UPDATE_POSITION			0x14CB


struct TradeRequest_Struct {
	/*00*/	uint32_t to_mob_id;
	/*04*/	uint32_t from_mob_id;
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