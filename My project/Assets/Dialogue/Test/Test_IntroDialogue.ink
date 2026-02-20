INCLUDE globals.ink
//INCLUDE Test_CampfireDialogue.ink
-> INTRO

=== INTRO ===
"Hi there! Let's get to know you a bit." #characterName:???
-> q1

=== q1 ===
"What best describes you?"

* I'm a night owl. #characterName:You
~ INTRO_sleepHabit = "night owl"
    "How original." #characterName:???
* I'm a morning person. #characterName:You
~ INTRO_sleepHabit = "morning person"
    "Wow, healthy. I like that!" #characterName:???
* I don't know. #characterName:You
~ INTRO_sleepHabit = "no preference"
    "Well, that's quite a dull answer, isn't it?" #characterName:???

- ~ R_INTRO_sleepHabit++
-> q2
   
=== q2 ===
"What kind of food do you like to eat?"

* I'm a vegetarian. #characterName:You
~ INTRO_diet = "vegetarian"
    "Riiiight... 'Vegetarian.'" #characterName:???
* Big fan of meat. #characterName:You
~ INTRO_diet = "carnivore"
    "That's <i>one</i> way to put it. I guess shame isn't a concept to you. #characterName:???
* [I don't have a preference.] Whatever makes me feel full. #characterName:You
    ~ INTRO_diet = "no preference"
    "I can't really fault you for that mindset." #characterName:???
    
- ~R_INTRO_diet++
-> q3 
 
 === q3 ===
 "One last question: Why do you seek the Grail?" #characterName:???
 
 * I want to help others. #characterName:You
     ~ INTRO_intentions = "people-pleaser"
     {INTRO_diet == "carnivore":
        "You're full of shit." #characterName:???
      - else:
        "How admirable." #characterName:???
    }
 * I want to get revenge. #characterName:You
 ~ INTRO_intentions = "revengeful"
 {INTRO_diet == "carnivore" && INTRO_sleepHabit == "night owl":
    "Again, very original. Are you half demon, too?" #characterName:???
  - else:
    "Okay, edgelord!" #characterName:???
}

 * I want to know who I am. #characterName:You
 ~INTRO_intentions = "amnesiac"
 {INTRO_sleepHabit == "no preference" && INTRO_diet == "no preference":
    "You're full of mystery, huh?" #characterName:???
  - else:
    "That's...a bit vague." #characterName:???
}
 
 - ~R_INTRO_intentions++ 
 "...Well, I hope you remember all of that. You'll need to keep your story straight if you want people to think you're human.  #characterName:???
 "Those kinds of folk don't take too kindly to flesh-eating monsters such as yourself."
 "<i>Especially</i> those looking to claim the Grail."

-> END