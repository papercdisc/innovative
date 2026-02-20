INCLUDE globals.ink

-> Start
===Start===
"...So then I told that bastard to choke on my fist!" #characterName:Cliff
"Wow, Cliff... You're so cool!" #characterName:Lace
Lace beams with admiration. Ilias, on the other hand, is struggling not to roll his eyes. #characterName:Narrator
"<i>Yeaah</i>... riveting stuff." #characterName:Ilias
"..." #characterName:Octavia
"Do we have a <i>problem</i> Ilias? Where is that tone coming from." #characterName:Cliff
"Nowhere. Relax, man." #characterName:Ilias
"Save your temper for the battlefield, Cliff." #characterName:Octavia
"I blame it on his empty stomach." #characterName:Ilias
"Sorry, guys. Food is just about ready!" #characterName:Lace
Soon enough, Lace is handing you a bowl of stew.#characterName:Narrator
You notice the presence of a few chunks of meat. They look incredibly tender.

* Take a bite.
    It practically melts in your mouth. Unfortunately, you know it won't do much to sate your hunger.
    {INTRO_diet == "vegetarian":
    "Oh my gosh, I'm so sorry. I gave you the wrong bowl!" #characterName:Lace
    "Doesn't look like it, from the way they're scarfing it down." #characterName:Ilias
    "Oh, I thought... Nix told me they were vegetarian. Did I hear wrong?" #characterName:Lace 
    ~Trust_Lace--
    ~Trust_Octavia--
    ~R_INTRO_diet--
    
  - else:
    "This is delicious, Lace." #characterName:Nix
    It's partially true. #characterName:Narrator
    "Thank you! I used to make this for my sister all the time." #characterName:Lace
    "Until... Well..."
    Lace goes silent. #characterName:Narrator
    After finishing all of the meat, you toss the rest when no one is looking.
    ~Trust_Lace++
    ~Trust_Cliff += 0.5
    ~R_INTRO_diet++
    }
    
    -> DONE
    
* Abstain.
    You decide not to eat any, despite how your stomach is growling. #characterName:Narrator
    {INTRO_diet == "vegetarian":
    "Oh my gosh, I'm so sorry. I gave you the wrong bowl!" #characterName:Lace
    "I was starting to wonder why my bowl didn't have any meat in it!" #characterName:Cliff
    "Hehe... Sorry..." #characterName:Lace
    "No worries." #characterName:Nix
    After she switches your bowls, you gingerly take a bite. #characterName:Narrator
    It tastes like dirt. It's not something you can blame her cooking on---most everything tastes this way.
    You barely manage to keep it down.
    
    ~Trust_Lace += 1.5
    ~R_INTRO_diet++
    -> DONE
  
  - else:
    "Are you really not going to eat anything, Nix? Come on, kid, put some meat on those bones!" #characterName:Cliff
    "Sorry. Just not feeling very hungry." #characterName:Nix
    "Can you at least try it? Just a bite? I-Is my cooking that bad?" #characterName:Lace
    
    Her eyes start to well up with tears. #characterName:Narrator
    
        ** Take a bite.
        "Okay! Okay..." 
        You take a bite. It's fine. At least the texture is good.#characterName:NULL
        "How is it?" #characterName:Lace
            *** "Delicious." #characterName:Nix
                It's partially true. #characterName:Narrator
                "Thank you! I used to make this for my sister all the time." #characterName:Lace
                "Until... Well..."
                Lace goes silent. 
                ~Trust_Lace += 0.5
                ~Trust_Cliff += 0.5
                ~R_INTRO_diet++
                -> DONE
            *** "It could use more salt." #characterName:Nix
                "Well... At least you tried it." #characterName:Lace
                You can tell she's trying not to look too disappointed.#characterName:NULL
                -> DONE
        ** Double down.
            "Sorry. I really can't." 
            "..." #characterName:Lace 
            Rather than start to cry, Lace wears a blank expression. #characterName:Narrator
            ~Trust_Lace -= 1
            -> DONE
    }
[WIP]
-> END