EXTERNAL OnUnlockSlot()
EXTERNAL OffUnlockSlot()
EXTERNAL OnEventUnlockSlot(index)
EXTERNAL GetGift(index)

VAR q_count = 0

-> choice_start

// // [[[ DEPTH == 0 ]]]
    === adv_forest_0 ===
        Under the warm sunlight, various flowers are fluttering. The trees make a noise with the wind.#name:none #img:none
        Welcome to the forest. #name:Mouse #img:mouse_idle
    -> choice_start
    
    === adv_sea_0 ===
        The cold air grazed my cheek. The smell of salty sea water fills my nose. #name:none #img:none
        Welcome to the winter sea. #name:Mouse #img:mouse_idle
    -> choice_start
    
    === adv_mine_0 ===
        I need a mining hat to explore this place. #name:Mouse #img:mouse_idle
        You brought it well! #name:Mouse #img:mouse_smile
        When you turn on the light of the mine hat, you can see a mine decoMouseed with wooden walls. I hear water dripping from somewhere. #name:none #img:none
    -> choice_start2
    
    === choice_start ===
        Shall we go? #name:Mouse #img:mouse_idle
        *   [Going forward]
            -> random_start
        +   [Talking to the mouse]
            Huh? You mean me? #name:Mouse #img:mouse_idle
            ->talk_mouse
    -> END
    
    === choice_start2 ===
        Shall we go? #name:Mouse #img:mouse_idle
        *   [Going forward]
            -> random_start
        +   [Talking to the mouse]
            Huh? You mean me? #name:Mouse #img:mouse_idle
            ->talk_mouse
        +   [What's this?]
            -> talk_mouse2
    -> END
    
    === talk_mouse ===
        What's going on? #name:Mouse #img:mouse_idle
        +   [Giving a gift]
            -> choice_mouse
        +   [Nothing]
            ...Huh? Okay. #name:Mouse #img:mouse_idle
            Then let's go on an adventure! #img:mouse_smile
    -> END
    
    === talk_mouse2 ===
        Looking closely at the bottom of the mine, <b>note</b> is off. #name:none #img:none
        <i>It says, "Come back here alone." #name:none
        What are you doing stopping there? Now let's start exploring! #name:Mouse #img:mouse_idle
    ->END
    
    === choice_mouse ===
        ~ OnEventUnlockSlot(3)
        (Looking for a gift.) #name:none #img:none
        +   [Going back]
            ~ OffUnlockSlot()
            -> talk_mouse
    -> END
    
    === answer_mouse ===
        ~ GetGift(3)
        Hup. #name:Mouse #img:mouse_surprise
        Oh, my... Are you really giving it to me?????
        ........... ........... #img:mouse_idle
        Thank you very much. You're the first person to give me a present.
        ...........
        Then... Just a little bit...
        I'll taste this!!!! #img:mouse_smile
        Don't venture while I taste this!!</b> 
        No way <b>!!</b>...okay?
        The mouse disappeared like lightning. #name:none #img:none
        Now, even if I move, the mouse doesn't follow me.</b>
    -> END
    
    === random_start ===
        #name:Mouse #img:mouse_smile
        { shuffle:
        	-   All right, are you ready? Now let's go on an adventure!
        	-   I'm not sure what's in here, but let's look it up together!
        	-   You're gonna try anything! If you fail, you'll have a better idea.
        	-   All right, let's start a fun adventure!
        	-   What will come out of this place? Let’s start looking!
        }
    ->END 
    
// // [[[ Common function ]]]
    === choice_common ===
        What should I do? #name:none #img:none
        +   [Use the item]
            -> ask_back_use
        +   [Staying here]
            -> ask_back_stay
        +   [Something's wrong]
            -> qurious
            
    -> END
    
// 1. Use things.
    === ask_back_use ===
        (The lifting status is not maintained. It locks again when the day changes.) #name:none #img:none
        ~ OnUnlockSlot()
        (It is used when the appropriate item is placed in the item compartment.)
        + [Going back]
            ~ OffUnlockSlot()
            -> choice_common
    -> END
    
    // // from external function : ADV-UnlockSlot()
    === use_stuff ===
        OK. Now we can go even deeper! #name:Mouse #img:mouse_smile
    -> END
    
    // 2. [Staying here]
    === ask_back_stay ===
        You don't have anything? Then it's good to stay! #name:Mouse #img:mouse_idle
        (This selection remains until the next day.) #name:none #img:none
        +   [Thinking again]
            -> choice_common
        +   [Staying here]
            ~ OffUnlockSlot()
            Yeah, let's not go any deeper, let's look around here! #name:Mouse #img:mouse_idle
            -> DONE
    -> END
    
    // 3. [Something's wrong]
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
        Hmm?  #name:Mouse #img:mouse_idle
        Something's wrong?
        .....
        ..............
        ............................
        No way. Let's focus on the expedition! #name:Mouse #img:mouse_smile
    -> choice_common
    
    == qurious_1 ===
        What else?  #name:Mouse #img:mouse_idle
        .....
        ..............
        Let's focus on exploring! #name:Mouse #img:mouse_smile
    -> choice_common
    
    === qurious_2 ===
        .... #name:Mouse #img:mouse_idle
        .......... .......... #img:mouse_confuse
        ................
        Let's focus on the expedition...! #name:Mouse
    -> choice_common
    
    === qurious_3 ===
        It can be weird that I'm following you around! Is there!? #name:Mouse #img:mouse_confuse
        I'll stare at you!
        He's following me around!
        Keep an eye on him!
        What did I say? #img:mouse_idle
        (The Mouse is suspicious). #name:none #img:none
    -> choice_common
    
    === qurious_4 ===
        ...Let's really move forward now!!! #name:Mouse #img:mouse_idle
    -> choice_common

    // // [[[ Forest. DEPTH == 10 || 20 ]]]
    === adv_forest_10 ===
        { shuffle:
            -   I used to smell fresh flowers
            -   past countless branches
            -   Enjoying the warm sunshine
        } <> You faced the wall. #name:none #img:none
        Oh, it's a wooden wall! #name:Mouse #img:mouse_surprise
        I'm stuck in a wall. I can't go any further... #img:mouse_confuse
        You need an axe to get through here. #name:Mouse #img:mouse_idle
    -> choice_common
    
    === adv_forest_20 ===
        { shuffle:
            -   Through dense trees
            -   I tasted the sweetness of sugar cane
        } <> A mouse talked to you. #name:none #img:none
        I can smell it. #name:Mouse #img:mouse_surprise
        The smell of a special plant!!!!
        I think I can see it if I have special glasses! #name:Mouse #img:mouse_smile
    -> choice_common

// // [[[ Sea.  DEPTH == 10 || 20 ]]]
    === adv_sea_10 ===
        { shuffle:
            -   When I stepped on the sand that was breaking down
            -   When I heard the calm waves
            -   I used to pick up cute shells
        } You curled up in the cold air. #name:none #img:none
        Oh, it's cold! #name:Mouse #img:mouse_surprise
        I can't go anymore because it's cold... #img:mouse_confuse
        You need something warm to get through here... ...lol... #name:Mouse
    -> choice_common
    
    === adv_sea_20 ===
        { shuffle:
            -   I almost slipped on the ice
            -   I used to make hair with seaweed
            -   I tasted the salt out of curiosity and spit it out
        } <> A mouse talked to you. #name:none #img:none
        This feeling!!! #name:Mouse #img:mouse_surprise
        There's something buried in the snow!!! I feel it on my little foot!!
        I think I can find it if I have special gloves! #name:Mouse #img:mouse_smile
    -> choice_common
    
// // [[[ Mine. DEPTH == 10 || 20 ]]]]
    // // same with 20
    === adv_mine_10 ===
        { shuffle:
            -   I was looking at the glittering ore
            -   I used to carefully pass through a pile of stones
            -   I was thinking about fried eggs while looking at the running oil
        } <> You faced a huge ore. #name:none #img:none
        Wow... it's huge! #name:Mouse #img:mouse_surprise
        It's bigger than you? Phew, you need to grow taller #name:Mouse #img:mouse_smile
        You glare at a mouse. #name:none
        ...... ...... #name:Mouse #img:mouse_smile
        Hehe...I need something to dig this up.#name:Mouse #img:mouse_smile
    -> choice_common
    
    === adv_mine_20 ===
        { shuffle:
            -   I was looking at the shiny ore
            -   I used to carefully pass through a pile of stones
            -   I was thinking about fried eggs while looking at the running oil
        } <> You faced a huge ore. #name:none #img:none
        Uh........  #name:Mouse #img:mouse_surprise
        Wow...............
        I can dig it......
        ....can you figure it out of course. #name:Mouse #img:mouse_confuse
        A mouse sneaks into your arms. #name:none
        He smiles awkwardly when his eyes meet. #name:none #img:mouse_smile
    -> choice_common