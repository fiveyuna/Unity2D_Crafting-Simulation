// index : 운디네-차분,운디네-활발,두더지,쮝
EXTERNAL OnUnlockSlot()
EXTERNAL OffUnlockSlot()
EXTERNAL OnEventUnlockSlot(index)
EXTERNAL GetGift(index)


VAR is_stranger = false // true: 수상한자와 이미 대화를 나눔.

// // depth : 30
=== adv_forest_30 ===
    As we get closer, we see a small tunnel. #name:none #img:none
    What is this? <b>Did a mole pass by? #name:Mouse #img:mouse_idle
    <b>The mole</b> lives deep in the mine... I've never seen it before!
    Oh, and!!! A mole will give you something sweet!!!! I really like sweet things!!! #img:mouse_smile
    If I get that as a gift... <b>I'm not going anywhere and I'm going to eat the candy little by little.
    정말..... #img:mouse_idle
    <b>I'm not going anywhere...
    ........
    Anyway, I can't go any deeper here! #img:mouse_smile
-> END

=== adv_sea_30 ===
    <i>Hi...</i>@<b>Hi!</b> #name:undyne #img:undine
    <i>This voice is only for you...</i>@<b>I can listen to you!</b> #name: It's lucky
    <i>If there's anything I want to give you...</i>@<b> Give me what you want!</b> #name: It's lucky
    (Who should I give a present to?) #name:none #img:none
    +   [To a calm friend]
        -> choice_undine_good
    +   [To a lively friend]
        -> choice_undine_bad
    +   [I have nothing to give you]
        - <i>I see...</i> <b> What?</b> #name: It's lucky
        - <i>See you again...</i><b>Hmph, I can't help it</b>#name: It's funny
-> END

    === choice_undine_good ===
        <i>What do you mean a present...</i> #name:undyne #img:undine
        ~ OnEventUnlockSlot(0)
        <i>Thank you in advance...</i>
        +   [Going back]
            ~ OffUnlockSlot()
            -> adv_sea_30
    ->END
    
    // When I got it as a gift 
    === answer_undine_good ===
        ~ GetGift(0)
        <i>...!!!</i> #name:undyne #img:undine
        It's so beautiful...
        Thank you very much. You made me happy!
        This is my reward. <b>Check your bag.</b>
    -> END
    
    === choice_undine_bad ===
        <b>[Laughing]! Gift?</b> #name:undyne #img:undine
        ~ OnEventUnlockSlot(1)
        <b>I'll give you a good present!</b>
        +   [Going back]
            ~ OffUnlockSlot()
            -> adv_sea_30
    ->END
    
    // When I got it as a gift 
    === answer_undine_bad ===
        ~ GetGift(1)
        <b>Yes, this is it! This!!!</b> #name:undyne #img:undine
        Laughing out loud! Thank you, thank you!
        This is something I cherish <b>I'll give it to you specially</b>! <b>Check your bag~</b>
        Use to make a <b>fuss</b> about someone!
    -> END

=== adv_mine_30 ===
    Oh my god... Isn't there one here... #name:dodaji #img:mole
    What a mole! #name:Mouse #img:mouse_surprise
    Oh, my! You're a young man, aren't you...And <b>that Mouse</b>.#name:dodaji #img:mole
    What? #name:Mouse #img:mouse_idle
    No. Well, it's hard for a human to come this far... I don't have a lot. #name:dodaji #img:mole
    Hmm. That's unique...It's a hassle, too.
    Well, if you have it, it's a different story.
    +   [I have it]
        -> choice_mole
    +   [I have nothing to give]
        ~ OffUnlockSlot()
        Hmm. That's too bad...
        If you have it, please feel free to look for it.
        -> END
-> END
    === choice_mole ===
        Oh! #name:dodaji #img:mole
        Oh!!!!!!!!!
        Oh, my God. I've been waiting for someone as cool as you.
        <b>Give it to me and I'll give you some valuable information.
        Oh, and... We'll give you something sweet.
        ...sweet things!!!! #name:Mouse #img:mouse_surprise
        ....just to appease the bitter truth. #name:dodaji #img:mole
        ~ OnEventUnlockSlot(2)
        Now, show us your figure!
        +   [Actually, there's nothing]
            ~ OffUnlockSlot()
            Ooh.
            Well, I'm not disappointed.
            Yeah, well, aren't you disappointed at all?
            Next... I'll promise...
    -> END
    
    // When I got it as a gift 
    === answer_mole ===
        ~ GetGift(2)
        Oh!!!!!!!!!!!!!! #name:img:mole
        <b>Diamond!!!!!!!!</b>
        Look at this solid surface... It's so brilliant, it's emitting a light that can't be compared to anything else, and if you collect that pleasant sparkle, you can fill the vast sky... The innumerable rays of light you can be felt inside...
        ...well, hold on. <b>That's... Won't you give it to me? #name:Mouse #img:mouse_idle
        ...Oh, no. I lost my mind for a moment. I apologize. #name:dodaji #img:mole
        As you know, this is where <b>magic becomes power</b>. The wizard holds a lot of things.
        It was the alchemist who challenged that authority.
        Especially your <b>teacher</b>... There were a lot of problems.
        It was <b>challenge</b> for wizards.
        ................. ................. #name:Mouse #img:mouse_idle
        ....it's hard to say more.  #name:dodaji #img:mole
        I put <b>comforting </b> in my bag. Thank you very much for your kindness.
        ...And don't trust a close person.
        (Dodgers disappeared after giving them things.) #name:none #img:none
        ........  ........  #name:Mouse #img:mouse_confuse
        Hehe...
        ...갈까? #img:mouse_idle
    -> END


////////// ////////// Mouse off /////////////

// Meeting the Stranger for the First Time O
=== off_mine_0_false === 
    ...You're here. #name:?? #img:none
    I've been waiting for you. His disciple. #name:Stranger #img:stranger
    You left the <b>Mouse</b> behind. It sounds good.
    ...Oh, I don't know where to start.
    -> choice_stranger
-> END

// Meeting the Stranger for the First Time X
=== off_mine_0_true ===
    ~ is_stranger = true
    I see you again. #name:Stranger #img:stranger
    -> choice_stranger_true
-> END

=== choice_stranger_true ===
    Is there anything you want to hear again? #name:Stranger #img:stranger
    +   [Story]
        -> choice_stranger
    +   [Please]
        -> quest_stranger
    +   [There's none]
        ...I wish you the best of luck.
-> END

=== choice_stranger ===
    Is there anything you want to hear?
    +   [About the current situation]
        Now everything is <b>possessed by the wizards</b>.
        Money, power, freedom...
        It's getting harder for those who can't do magic without mana by nature.
        Your teacher, Basic, was doing a forbidden study.
        So that everyone can handle the mana that only the wizard can handle... <b>You were making the ultimate material.
        The result was <b>successful</b>.
        ...But this news came to the wizards, too.
        The wizards were afraid of their power being divided.
        They wanted to send him somewhere to stop him, and they wanted to take the ultimate material, but...
        Just before you left, <b>The research tool was removed</b> and it became impossible.
        In order to recreate the research tool, we had to interpret the equation
        Wizards who couldn't interpret alchemists' formulas...
        Found an alchemist who can interpret formulas.
        ...That's <b>you</b>.
        -> choice_stranger
    +   [About Mouses]
        The mouse is <b>the sorcerer's assistant</b>.
        Follow your actions <b>and <b>watch</b>and deliver them to the wizards.
        I don't know how you got away with it, but... You've done a nice job.
        -> choice_stranger
    +   [There's none]
        {   is_stranger :
            -> choice_stranger_true 
        - else : 
            -> quest_stranger
        } 
        
        
-> END

=== quest_stranger ===
    I'll ask you a favor. #name:Stranger #img:stranger
    <b>Keep making research tools so that mice don't doubt them.</b>
    And meet <b>undine</b> at <b>sea</b> and give <b>treasure</b>. Then you'll get the drugs.
    Use it in <b>the decisive moment</b> when the research tool is completed.
    If we succeed, we can keep our research.
    Please... <b>Protect the future of alchemy.</b>
    {   is_stranger:
            -> choice_stranger_true
    - else:
        ~ GetGift(4)
        I'm always hiding here.
        If you have any questions, please come back.
        ...I wish you the best of luck.
    }
-> END


// // === adv_mine_30_true ===
//     Dodaji was observing the jewels so hard that he didn't notice your existence. #name:none #img:mole
// // -> END

// // [[[ Common function - mouse off ]]]
=== choice_common_true ===
    What should I do? #name:none #img:none
    +   [Use the item]
        -> ask_back_use_true
    +   [Staying here].
        -> ask_back_stay_true
        
-> END

// 1. Use things.
=== ask_back_use_true ===
    (The lifting status is not maintained. It locks again when the day changes.) #name:none #img:none
    ~ OnUnlockSlot()
    (It is used when the appropriate item is placed in the item compartment.)
    + [Going back]
        ~ OffUnlockSlot()
        -> choice_common_true
-> END

// // from external function : ADV-UnlockSlot()
=== use_stuff ===
    Now we can go deeper. #name:none #img:none
-> END

// 2. [Staying here].]
=== ask_back_stay_true ===
    (This selection remains until the next day.) #name:none #img:none
    +   [Thinking again]
        -> choice_common_true
    +   [Staying here]
        ~ OffUnlockSlot()
        You decided to stay here. #name:none #img:none
        -> DONE
-> END