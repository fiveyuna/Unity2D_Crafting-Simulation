EXTERNAL OnUnlockSlot()
EXTERNAL OffUnlockSlot()
EXTERNAL OnEventUnlockSlot(index)
EXTERNAL GetGift(index)

VAR q_count = 0

-> choice_start


// [[[ DEPTH == 0 ]]]
    === adv_forest_0 ===
        따사로운 햇살 아래,@다양한 꽃들이 살랑이고 있다.@나무들이 바람을 따라 소리를 낸다.#name:none #img:none
        숲에 도착한 걸 환영해. #name:쥐 #img:mouse_idle
    -> choice_start
    
    === adv_sea_0 ===
        차가운 공기가 뺨을 스친다.@짠 바닷물의 냄새가 코를 가득 채운다. #name:none #img:none
        겨울바다에 도착한 걸 환영해. #name:쥐 #img:mouse_idle
    -> choice_start
    
    === adv_mine_0 ===
        이곳을 탐험하려면 광산 모자가 필요해. #name:쥐 #img:mouse_idle
        마침 잘 들고 왔구나! #name:쥐 #img:mouse_smile
        광산 모자의 불을 켜자@나무 벽으로 꾸며진 광산이 보인다.@어디선가 물 떨어지는 소리가 들린다. #name:none #img:none
    -> choice_start2
    
    === choice_start ===
        출발해 볼까? #name:쥐 #img:mouse_idle
        *   [앞으로 간다.]
            -> random_start
        +   [쥐에게 말을 건다.]
            응? 나 말이야? #name:쥐 #img:mouse_idle
            ->talk_mouse
    -> END
    
    === choice_start2 ===
        출발해 볼까? #name:쥐 #img:mouse_idle
        *   [앞으로 간다.]
            -> random_start
        +   [쥐에게 말을 건다.]
            응? 나 말이야? #name:쥐 #img:mouse_idle
            ->talk_mouse
        +   [이건 뭐지?]
            -> talk_mouse2
    -> END
    
    === talk_mouse ===
        무슨 일이야? #name:쥐 #img:mouse_idle
        +   [선물을 준다]
            -> choice_mouse
        +   [아무것도 아냐]
            ...응? 알았어. #name:쥐 #img:mouse_idle
            그럼 이제 모험을 떠나자! #img:mouse_smile
    -> END
    
    === talk_mouse2 ===
        광산 바닥을 자세히 보니@<b>쪽지</b>가 떨어져 있다. #name:none #img:none
        <i>'혼자 이곳으로 다시 올 것'</i> @...라고 적혀있다. #name:none
        거기 멈춰서 뭐 해?@이제 탐험을 시작하자! #name:쥐 #img:mouse_idle
    ->END
    
    === choice_mouse ===
        ~ OnEventUnlockSlot(3)
        (선물을 찾아본다.) #name:none #img:none
        +   [돌아가기]
            ~ OffUnlockSlot()
            -> talk_mouse
    -> END
    
    === answer_mouse ===
        ~ GetGift(3)
        헉. #name:쥐 #img:mouse_surprise
        헉 세상에...@정말 나한테 주는 거야?????
        ........... #img:mouse_idle
        정말.... 고마워.@나에게 선물을 준 건 네가 처음이야.
        ...........
        그럼... 나 조금만...
        이거 맛보고 있을게!!!!!! #img:mouse_smile
        내가 이거 맛볼 동안@<b>모험하면 안 돼!!</b> 
        절대로 <b>안 돼!!</b> @...알겠지?!
        쥐는 번개처럼 사라졌다. #name:none #img:none
        이제 움직여도@<b>쥐가 따라오지 않는다.</b>
    -> END
    
    === random_start ===
        #name:쥐 #img:mouse_smile
        { shuffle:
        	-   자, 준비됐어?@이제 모험을 떠나보자!
        	-   이곳에 뭐가 있는지 잘 모르겠지만,@같이 찾아보자!
        	-   뭐든지 시도해 보는 거야!@실패하면 더 나은 아이디어가 나올 거야.
        	-   좋아, 즐거운 모험을 시작해 보자!
        	-   이 곳에선 뭐가 나올까?@어서 찾아보자!
        }
    ->END 
    
// [[[ Common function ]]]
    === choice_common ===
        어떻게 할까? #name:none #img:none
        +   [물건을 사용한다.]
            -> ask_back_use
        +   [이곳에 머문다.]
            -> ask_back_stay
        +   [뭔가 이상하다.]
            -> qurious
            
    -> END
    
    // 1. 물건을 사용한다.
    === ask_back_use ===
        (해금 상태는 유지되지 않습니다.@날이 바뀌면 다시 잠깁니다.) #name:none #img:none
        ~ OnUnlockSlot()
        (적절한 물건을@아이템 칸에 두면 사용됩니다.)
        + [돌아가기]
            ~ OffUnlockSlot()
            -> choice_common
    -> END
    
    // from external function : ADV-UnlockSlot()
    === use_stuff ===
        좋아.@이제 더 깊은 곳까지 갈 수 있어! #name:쥐 #img:mouse_smile
    -> END
    
    // 2. [이곳에 머문다.]
    === ask_back_stay ===
        혹시 물건이 없어?@그럼 머무는 것도 좋지~! #name:쥐 #img:mouse_idle
        (이 선택은 다음 날이@되기 전까지 유지됩니다.) #name:none #img:none
        +   [다시 생각한다]
            -> choice_common
        +   [이곳에 머문다]
            ~ OffUnlockSlot()
            그래, 더 깊이 가지 말고@이 주변을 둘러보자! #name:쥐 #img:mouse_idle
            -> DONE
    -> END
    
    // 3. [뭔가 이상하다.]
    === qurious ===
        ~ q_count++
        { 
            - q_count == 1: -> qurious_0
            - q_count == 2: -> qurious_1
            - q_count == 3: -> qurious_2
            - q_count == 4: -> qurious_3
            - else: -> qurious_4
        }
    -> END
    
    == qurious_0 ===
        음?  #name:쥐 #img:mouse_idle
        뭔가 이상하다고?
        .....
        ..............
        ............................
        에이~ 그럴 리가.@탐험에 집중하자! #name:쥐 #img:mouse_smile
    -> choice_common
    
    == qurious_1 ===
        또?  #name:쥐 #img:mouse_idle
        .....
        ..............
        에이~ @탐험에 집중하자! #name:쥐 #img:mouse_smile
    -> choice_common
    
    == qurious_2 ===
        .... #name:쥐 #img:mouse_idle
        .......... #img:mouse_confuse
        ................
        탐험에 집중하자...! #name:쥐
    -> choice_common
    
    === qurious_3 ===
        내, 내가 졸졸 따라다니는 게@이상할 수! 있지!@...있나? #name:쥐 #img:mouse_confuse
        내, 내가 뚫어져라 보고!
        쫓아다니고!
        감시하고!
        ...어? 내가 뭐라고 했더라? #img:mouse_idle
        (쥐가 수상하다.@따돌려야 할 것 같다.) #name:none #img:none
    -> choice_common
    
    === qurious_4 ===
        ...이제 정말 앞으로 가자!!! #name:쥐 #img:mouse_idle
    -> choice_common
    

// [[[ Forest. DEPTH == 10 || 20 ]]]
    === adv_forest_10 ===
        { shuffle:
            -   싱그러운 꽃향기를 맡던
            -   무수한 나뭇가지를 지난
            -   따듯한 햇살을 만끽하던
        } <> @당신은 벽과 마주했다. #name:none #img:none
        헉, 나무 벽이잖아! #name:쥐 #img:mouse_surprise
        벽에 막혀서 더 이상 갈 수 없어... #img:mouse_confuse
        이곳을 지나려면@도끼가 필요해. #name:쥐 #img:mouse_idle
    -> choice_common
    
    === adv_forest_20 ===
        { shuffle:
            -   빽빽한 나무를 지나던
            -   사탕수수의 달콤함을 맛보던
        } <> @당신에게 쥐가 말을 걸어왔다. #name:none #img:none
        냄새가 난다, 냄새가...! #name:쥐 #img:mouse_surprise
        특별한 식물의 냄새가!!!!
        특별한 안경이 있다면@볼 수 있을 것 같아! #name:쥐 #img:mouse_smile
    -> choice_common

// [[[ Sea.  DEPTH == 10 || 20 ]]]
    === adv_sea_10 ===
    { shuffle:
        -   사르륵 부서지는 모래를 밟던
        -   잔잔한 파도 소리를 듣던
        -   귀여운 조개껍데기를 줍던
    } <> @당신은 추운 공기에 몸을 웅크렸다. #name:none #img:none
    앗, 추워...! #name:쥐 #img:mouse_surprise
    추워서 더는 갈 수 없어어더덜... #img:mouse_confuse
    이곳을 지나려면@따듯한 물건이 필요해...@...훌쮜럭.... #name:쥐
    -> choice_common
    
    === adv_sea_20 ===
    { shuffle:
        -   얼음 위에서 미끄러질 뻔한
        -   해초로 머리카락을 만들며 놀던
        -   호기심에 소금을 맛보고 바로 뱉은
    } <> @당신에게 쥐가 말을 걸어왔다. #name:none #img:none
    이 느낌은!!! #name:쥐 #img:mouse_surprise
    눈 속에 무언가 파묻혀 있어!!!@내 작은 발을 타고 느껴져!!
    특별한 장갑이 있다면@찾을 수 있을 것 같아! #name:쥐 #img:mouse_smile
    -> choice_common
    
// [[[ Mine. DEPTH == 10 || 20 ]]]]
    // same with 20
    === adv_mine_10 ===
    { shuffle:
        -   반짝거리는 광석을 구경하던
        -   조심조심 돌무더기를 지나던
        -   흐르는 기름을 보며@계란 프라이를 생각하던
    } <> @당신은 거대한 광석과 마주했다. #name:none #img:none
    우와.......엄청 크다 ! #name:쥐 #img:mouse_surprise
    너보다 큰 걸? 푸히힛@너도 키 좀 커야겠다~ #name:쥐 #img:mouse_smile
    당신은 쥐를 째려본다. #name:none
    ...... #name:쥐 #img:mouse_smile
    히히.....이걸 캐낼 수 있는@물건이 필요해.#name:쥐 #img:mouse_smile
    -> choice_common
    
    === adv_mine_20 ===
    { shuffle:
        -   반짝반짝거리는 광석을 구경하던
        -   조심조심 돌무더기를 지나던
        -   흐르는 기름을 보며@계란 프라이를 생각하던
    } <> @당신은 거어어어어어어어어어어어어어대한 광석과 마주했다. #name:none #img:none
    우........  #name:쥐 #img:mouse_surprise
    우와..............
    캘 수 있어!...........
    ......캘 수 있지...? #name:쥐 #img:mouse_confuse
    쥐가 슬그머니 당신의 품에 숨는다. #name:none
    눈이 마주치자 머쓱하게 웃는다. #name:none #img:mouse_smile
    -> choice_common
