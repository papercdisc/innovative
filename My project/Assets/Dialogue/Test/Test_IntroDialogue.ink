INCLUDE globals.ink
-> INTRO

=== INTRO ===
"Hi there! Let's get to know you a bit."
-> q1

=== q1 ===
"What best describes you?"

* I'm a night owl.
~ INTRO_sleepHabit = "night owl"
    "How original."
    -> q2
* I'm a morning person.
~ INTRO_sleepHabit = "morning person"
    "Wow, healthy. I like that!"
    -> q2
* I don't know.
~ INTRO_sleepHabit = "no preference"
    "Well, that's quite a dull answer, isn't it?"
   -> q2
   
=== q2 ===
"What kind of food do you like to eat?"

* I'm a vegetarian.
~ INTRO_diet = "vegetarian"
    "Riiiight... 'Vegetarian.'"
    -> q3
* Big fan of meat.
~ INTRO_diet = "carnivore"
    "That's <i>one</i> way to put it. I guess shame isn't a concept to you."
    -> q3
* [I don't have a preference.] Whatever makes me feel full.
    ~ INTRO_diet = "no preference"
    "I can't really fault you for that mindset."
    -> q3 
 
 === q3 ===
 "One last question: Why do you seek the Grail?"
 
 * I want to help others.
     ~ INTRO_intentions = "people-pleaser"
     {INTRO_diet == "carnivore":
        "You're full of shit."
      - else:
        "How admirable."
    }
 * I want to get revenge.
 ~ INTRO_intentions = "revengeful"
 {INTRO_diet == "carnivore" && INTRO_sleepHabit == "night owl":
    "Again, very original. Are you half demon, too?"
  - else:
    "Okay, edgelord!"
}

 * I want to know who I am.
 ~INTRO_intentions = "amnesiac"
 {INTRO_sleepHabit == "no preference" && INTRO_diet == "no preference":
    "You're full of mystery, huh?"
  - else:
    "That's...a bit vague."
}
 
 - "...Well, I hope you remember all of that. You'll need to keep your story straight if you want people to think you're human. 
 "Those kinds of folk don't take too kindly to flesh-eating monsters such as yourself."
 "<i>Especially</i> those looking to claim the Grail."

-> END