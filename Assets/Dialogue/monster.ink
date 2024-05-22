// index : 운디네-차분,운디네-활발,두더지,쮝

EXTERNAL OnUnlockSlot()
EXTERNAL OffUnlockSlot()
EXTERNAL OnEventUnlockSlot(index)
EXTERNAL GetGift(index)


VAR is_stranger = false // true: 수상한자와 이미 대화를 나눔.

// depth : 30
=== adv_forest_30 ===
    가까이 다가가자@작은 땅굴이 보인다. #name:none #img:none
    이게 뭘까?@<b>두더지</b>라도 지나간 걸까? #name:쥐 #img:mouse_idle
    <b>두더지</b>는 <b>광산</b> 깊은 곳에 산다던대...@한 번도 본 적은 없어!
    아 그리고!!!@두더지는 단 걸 준대!!!!@내가 단 걸 엄청 좋아하거든!!! #img:mouse_smile
    그걸 선물 받는다면...@<b>아무 데도 안 가고</b>@그 사탕을 조금씩 먹을 거야.
    정말..... #img:mouse_idle
    <b>아무 데도 안 가고</b> 말이야...
    ........
    아무튼, 여기서 더 깊게 갈 수 없어! #img:mouse_smile
-> END

=== adv_sea_30 ===
    <i>안녕...</i>@<b>안녕?</b> #name:운디네 #img:undine
    <i>이 목소리는 오직 너만...</i>@<b>들을 수 있지!</b> #name:운디네
    <i>주고픈게 있다면... 줘..</i>@<b>원하는 걸 줘!</b> #name:운디네
    (누구에게 선물을 줄까?) #name:none #img:none
    +   [차분한 친구에게]
        -> choice_undine_good
    +   [활발한 친구에게]
        -> choice_undine_bad
    +   [줄 것이 없다]
        - <i>그렇구나...</i>@<b>뭐~?</b> #name:운디네
        - <i>또 봐...</i>@<b>흥, 어쩔 수 없지</b>#name:운디네
-> END

    === choice_undine_good ===
        <i>선물이라니...</i> #name:운디네 #img:undine
        ~ OnEventUnlockSlot(0)
        <i>미리 고마워...</i>
        +   [돌아가기]
            ~ OffUnlockSlot()
            -> adv_sea_30
    ->END
    
    // 선물 받았을 때 
    === answer_undine_good ===
        ~ GetGift(0)
        <i>...!!!!!</i> #name:운디네 #img:undine
        이렇게 아름다운 걸...
        정말 고마워.@덕분에 행복해졌어!
        이건 내 보답이야.@<b>가방을 확인해봐.</b>
    -> END
    
    === choice_undine_bad ===
        <b>하하! 선물?</b> #name:운디네 #img:undine
        ~ OnEventUnlockSlot(1)
        <b>좋은 선물엔 나도 좋은 걸 줄게!</b>
        +   [돌아가기]
            ~ OffUnlockSlot()
            -> adv_sea_30
    ->END
    
    // 선물 받았을 때 
    === answer_undine_bad ===
        ~ GetGift(1)
        <b>그래, 이거야 이거!!!</b> #name:운디네 #img:undine
        파하하! 고마워, 고마워!
        이건 내가 아끼는 건데@<b>특별히</b> 줄게!@<b>가방을 확인해 봐~</b>
        누군가를 <b>골탕먹일 때</b>@사용하도록 해!
    -> END
    
=== adv_mine_30 ===
    호오... 여기도 없나... #name:도다지 #img:mole
    헉, 진짜 두더지잖아! #name:쥐 #img:mouse_surprise
    아! 어린 인간이군요?@...그리고 <b>그 쥐</b>라니.#name:도다지 #img:mole
    네...? #name:쥐 #img:mouse_idle
    아닙니다.@뭐, 인간이 여기까지 오는 일은...@잘 없는데 말이죠. #name:도다지 #img:mole
    흐음. 독특한 일이네요@...귀찮은 일이기도 하죠.
    뭐, <b>그걸</b> 갖고 있다면,@얘기가 달라집니다만.
    +   [갖고 있다]
        -> choice_mole
    +   [줄 선물이 없다]
        ~ OffUnlockSlot()
        흠. 아쉽군요...
        갖고 있다면 언제든 찾아주시길.
        -> END
-> END
    === choice_mole ===
        오! #name:도다지 #img:mole
        오!!!!!!!!!
        이런 세상에.@당신처럼 멋진 사람을 기다렸답니다.
        <b>그걸</b> 주시면@귀한 정보를 드리겠습니다.
        아, 그리고 뭐...@달달한 것도 드립니다.
        ...단 거!!!! #name:쥐 #img:mouse_surprise
        ....쓴 진실을 달래기 딱이죠. #name:도다지 #img:mole
        ~ OnEventUnlockSlot(2)
        자, 이제 그 자태를 보여주시죠!
        +   [사실 없다]
            ~ OffUnlockSlot()
            오.
            이런.@뭐......@실망한 건 아닙니다.
            네... 뭐......@전혀 실망하지 않았어요?
            다음을... 킁...@기약하지요.......
    -> END
    
    // 선물 받았을 때 
    === answer_mole ===
        ~ GetGift(2)
        오오오!!!!!!!!!!!!!!!!!!! #name:도다지 #img:mole
        <b>다이아몬드!!!!!!!!!</b>
        이 단단한 겉을 봐... 아주 영롱해애애애 어떤 것과도 비교할 수 없는 빛을 내뿜고 있어어어 그 유쾌한 반짝임을 모으면 드넓은 하늘도 가득 채울 수 있다구우우우 오오... 속에서 느껴지는 무수한 빛줄기...
        ...자, 잠깐만요.@<b>그거</b>... 안 주시나요? #name:쥐 #img:mouse_idle
        ...아, 이런.@제가 잠시 이성을 잃었군요.@사과의 말씀 드립니다. #name:도다지 #img:mole
        아다시피 이 곳은@<b>마법이 곧 힘</b>이 되는 곳입니다.@마법사가 많은 것을 쥐고 있지요.
        그 권위에 도전한 것이@<b>연금술사</b> 이지요.
        특히 당신의 <b>스승</b>은... 문제가 많았지요.
        마법사들에게@<b>골칫거리</b>였습니다.
        ................. #name:쥐 #img:mouse_idle
        .....더 얘기하긴 어렵겠군요.  #name:도다지 #img:mole
        가방에 <b>위로가 될 만한 것</b>을@넣었습니다. 당신의 베풂에@큰 감사를 드립니다.
        ...그리고 가까운 이를@<b>믿지 마시길</b>.
        (도다지는 물건을 주고 사라졌다.) #name:none #img:none
        ........  #name:쥐 #img:mouse_confuse
        헤헤...
        ...갈까? #img:mouse_idle
    -> END


////////// Mouse off /////////////

// 수상한 자와 첫 만남 O
=== off_mine_0_false === 
    ...오셨군요. #name:??? #img:none
    당신을 기다리고 있었습니다.@그의 제자여. #name:수상한자 #img:stranger
    그 <b>쥐</b>를 따돌리고 오셨군요.@좋습니다.
    ...이런, 어떤 얘기부터@해야 할지 모르겠군요.
    -> choice_stranger
-> END

// 수상한 자와 첫 만남 X
=== off_mine_0_true ===
    ~ is_stranger = true
    또 뵙는군요. #name:수상한자 #img:stranger
    -> choice_stranger_true
-> END

=== choice_stranger_true ===
    다시 듣고 싶은 것이 있으십니까? #name:수상한자 #img:stranger
    +   [이야기]
        -> choice_stranger
    +   [부탁]
        -> quest_stranger
    +   [없다]
        ...행운을 빕니다.
-> END

=== choice_stranger ===
    듣고 싶은 이야기가 있으십니까?
    +   [현 상황에 대해서]
        현재 모든 것은@<b>마법사들이 독차지</b>하고 있습니다.
        돈, 권력, 그리고 자유까지...
        선천적으로 마나가 없어 마법을 부릴 수 없는 자들은 더욱 힘들어지고 있죠.
        당신의 스승님, '기초로'님께선@<b>금단의 연구</b>를 하고 계셨습니다.
        마법사만이 다룰 수 있는 마나를@모두가 다룰 수 있도록...@<b>궁극의 물질</b>을 만들고 계셨죠.
        결과는 <b>성공적</b> 이었습니다.
        ...그러나 이 소식은@마법사들에게도 전해졌죠.
        마법사들은 자신들의 힘이 나눠지는 것을 두려워했습니다.
        그들은 기초로님을 막기 위해<b>@그를 어디론가 보내버리고</b>@궁극의 물질 또한 <b>독차지</b>하려 했으나...
        기초로님께서 떠나시기 직전,@<b>연구도구를 없애</b>@불가능하게 되었습니다.
        연구도구를 다시 만들기 위해선@<b>수식을 해석</b>해야 했고
        연금술사들의 수식을@해석할 수 없던 마법사들은...
        수식을 해석할 수 있는@연금술사를 찾았습니다.
        ...그게 <b>당신</b>입니다.
        -> choice_stranger
    +   [쥐에 대해서]
        그 쥐는 <b>마법사들의 조수</b>입니다.
        당신의 행동을 <b>추적</b>하고 <b>감시</b>해@마법사들에게 전달하죠.
        어떻게 따돌리신 건지@모르겠지만... 잘하셨습니다.
        -> choice_stranger
    +   [없다]
        {   is_stranger :
            -> choice_stranger_true 
        - else : 
            -> quest_stranger
        } 
        
        
-> END

=== quest_stranger ===
    당신께 부탁을 드리겠습니다. #name:수상한자 #img:stranger
    쥐가 의심하지 않도록@<b>연구도구를 계속 만드세요.</b>
    그리고 <b>바다</b>에서@<b>운디네</b>를 만나 <b>보물</b>을 주세요.@그럼 아주 쓴 약을 받을 수 있을 겁니다.
    그것을 연구도구가 완성되는,@<b>결정적인 순간</b>에 사용해 주세요.
    성공한다면@우리의 연구를 지킬 수 있습니다.
    부탁합니다...@<b>연금술의 미래를 지켜주세요.</b>
    {   is_stranger:
            -> choice_stranger_true
    - else:
        ~ GetGift(4)
        저는 항상 이곳에 숨어있습니다.
        궁금한 점이 있다면@다시 찾아와주세요.
        ...행운을 빕니다.
    }
-> END


// === adv_mine_30_true ===
//     도다지는 열심히 보석을 관찰하고 있어@당신의 존재를 눈치채지 못했다. #name:none #img:mole
// -> END

// [[[ Common function - mouse off ]]]
=== choice_common_true ===
    어떻게 할까? #name:none #img:none
    +   [물건을 사용한다.]
        -> ask_back_use_true
    +   [이곳에 머문다.]
        -> ask_back_stay_true
        
-> END

// 1. 물건을 사용한다.
=== ask_back_use_true ===
    (해금 상태는 유지되지 않습니다.@날이 바뀌면 다시 잠깁니다.) #name:none #img:none
    ~ OnUnlockSlot()
    (적절한 물건을@아이템 칸에 두면 사용됩니다.)
    + [돌아가기]
        ~ OffUnlockSlot()
        -> choice_common_true
-> END

// from external function : ADV-UnlockSlot()
=== use_stuff ===
    이제 더 깊은 곳까지 갈 수 있다. #name:none #img:none
-> END

// 2. [이곳에 머문다.]
=== ask_back_stay_true ===
    (이 선택은 다음 날이@되기 전까지 유지됩니다.) #name:none #img:none
    +   [다시 생각한다]
        -> choice_common_true
    +   [이곳에 머문다]
        ~ OffUnlockSlot()
        당신은 이곳에 머물기로 했다. #name:none #img:none
        -> DONE
-> END

