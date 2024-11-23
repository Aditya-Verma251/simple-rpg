import random
import time

random.seed()

#DO : ADD LEVELS TO THIS
#DONE

def statmultiplier():
    return round((0.75 + (0.5 * (random.random()))), 2)

#gameOver = False

PLAYERCLASSES = ('dps', 'tank', 'bal')
PLAYERSTATS = {
    PLAYERCLASSES[0] : (174, 449, 145),
    PLAYERCLASSES[1] : (386, 133, 249),
    PLAYERCLASSES[2] : (255, 220, 218)
}

MONSTERSPECIES = ('slime', 'wolf', 'vampire', 'dragon')
MONSTERSTATS = {
    MONSTERSPECIES[0] : (184, 157, 151),
    MONSTERSPECIES[1] : (429, 188, 267),
    MONSTERSPECIES[2] : (722, 372, 574),
    MONSTERSPECIES[3] : (1074, 1004, 994)
}


class actors:
    name = 'a'
    health = 0
    attack = 0
    defense = 0

class player(actors):
   
    playerClass = 'c'
    playerExperience = 0
    playerLevel = 1

    #some redundant code-
    '''def playerClassCheck(playerToBeChecked):
       if (playerToBeChecked.playerClass == PLAYERCLASSES[0]):
           pass
       elif (playerToBeChecked.playerClass == PLAYERCLASSES[1]):
           pass
       elif (playerToBeChecked.playerClass == PLAYERCLASSES[2]):
           pass
       else:
           print("Class check failed")'''

    def __init__(self, playerName, playerClassInput):
        self.name = playerName
        self.playerClass = playerClassInput
        self.health = int((statmultiplier()) * (PLAYERSTATS[self.playerClass][0]))
        self.attack = int((statmultiplier()) * (PLAYERSTATS[self.playerClass][1]))
        self.defense = int((statmultiplier()) * (PLAYERSTATS[self.playerClass][2]))

    def levelup(self):
        self.playerLevel += 1
        self.health += (10 + int(self.playerLevel * statmultiplier()))
        self.attack += (3 + int(self.playerLevel * statmultiplier()))
        self.defense += (3 + int(self.playerLevel * statmultiplier()))

        print("YOU LEVELED UP!!!!!!!!")
        print("Your stats now are:")
        print("NAME     :   ", self.name)
        print("CLASS    :   ", self.playerClass)
        print("LEVEL    :   ", self.playerLevel)
        print("HEALTH   :   ", self.health)
        print("ATTACK   :   ", self.attack)
        print("DEFENSE  :   ", self.defense)
        print('*******************************************************************')
        time.sleep(1.5)


    

class monster(actors):
    monsterSpecies = 's'
    monsterXP = 1

    def monsterSpeciesDecide():
        chance = random.random()
        if (chance <= 0.47):
            return 0
        elif (chance > 0.47 and chance <= 0.7):
            return 1
        elif (chance > 0.7 and chance <= 0.9):
            return 2
        elif (chance > 0.9):
            return 3
        else:
            print('something went wrong')

    def __init__(self):
        self.monsterSpecies = MONSTERSPECIES[monster.monsterSpeciesDecide()]
        self.name = self.monsterSpecies
        self.health = int((statmultiplier()) * (MONSTERSTATS[self.monsterSpecies][0]))
        self.attack = int((statmultiplier()) * (MONSTERSTATS[self.monsterSpecies][1]))
        self.defense = int((statmultiplier()) * (MONSTERSTATS[self.monsterSpecies][2]))
        self.monsterXP = ((MONSTERSTATS[self.monsterSpecies][0])+(MONSTERSTATS[self.monsterSpecies][1])+(MONSTERSTATS[self.monsterSpecies][2]))

def setup():
    #gameOver = False

    print('*******************************************************************')
    print("WELCOME TO THIS RPG I MADE BECAUSE CHATGPT TOLD ME IT WOULD HELP ME LEARN OOP")
    print('*******************************************************************')
    time.sleep(1)
    name_input = input("Enter your characte's name: ")
    print('.')
    time.sleep(0.5)
    print("Classes -  'dps' for better attack , 'tank' for better defense, and 'bal' for balanced")
    print('.')
    class_input = input("Enter your character's class: ")
    time.sleep(1)
    print('*******************************************************************')
    print("In this game, you just fight monters. Yeah that's all :P")
    print('*******************************************************************')
    print('.')
    time.sleep(3)

    p1 = player(name_input, class_input)

    print("INFO ABOUT YOUR CHARACTER:")
    print("NAME     :   ", p1.name)
    print("CLASS    :   ", p1.playerClass)
    print("LEVEL    :   ", p1.playerLevel)
    print("HEALTH   :   ", p1.health)
    print("ATTACK   :   ", p1.attack)
    print("DEFENSE  :   ", p1.defense)

    time.sleep(1)

    print('*******************************************************************')

    return p1

def fight(playerObject):
    print('*******************************************************************')
    monsterObject = monster()
    statRatio = float(playerObject.health + playerObject.attack + playerObject.defense)/float(monsterObject.health + monsterObject.attack + monsterObject.defense)

    print("You encountered a ", monsterObject.name)

    while (monsterObject.health > 0 and playerObject.health > 0):
        move = input("Would you like to attack or run? ")

        if (move == 'attack'):
            monsterObject.health -= int((((((MONSTERSTATS[MONSTERSPECIES[0]][0]+MONSTERSTATS[MONSTERSPECIES[1]][0]+MONSTERSTATS[MONSTERSPECIES[2]][0]+MONSTERSTATS[MONSTERSPECIES[3]][0])/4.0) * statRatio)) + (playerObject.attack - monsterObject.defense)) * statmultiplier())
            if (monsterObject.health <= 0):
                continue
        elif (move == 'run'):
            if (random.random() <= statRatio):
                print("You succesfully ran away")
                break
            else:
                print("You couldn't run away")
        else:
            print("Invalid Move")

        playerObject.health -= int((((((PLAYERSTATS[PLAYERCLASSES[0]][0]+PLAYERSTATS[PLAYERCLASSES[1]][0]+PLAYERSTATS[PLAYERCLASSES[2]][0])/3.0)/statRatio)) + (monsterObject.attack - playerObject.defense)) * statmultiplier())
        if (playerObject.health <= 0):
            print("You Lost")
            #gameOver = True
            break
    else:
        print('*******************************************************************')
        print("YOU WON!!!!!!")
        playerObject.health += (int(0.25 * (PLAYERSTATS[playerObject.playerClass][0])) + 1)
        playerObject.playerExperience += monsterObject.monsterXP

        if (playerObject.playerExperience >= ((playerObject.playerLevel ** 3) * 1000)):
            playerObject.levelup()
        
        print("Your current health is ", playerObject.health)
        print("Your current level is ", playerObject.playerLevel)
        time.sleep(1)
        print('*******************************************************************')


playerCharacter = setup()
while True:
    fight(playerCharacter)
    if (playerCharacter.health <= 0):
        print("GAME OVER")
        time.sleep(3)
        break


'''
p1 = player("P1", "dps")

m1 = monster()

print(p1.name)
print(PLAYERSTATS['bal'][0])

print(statmultiplier())
print(p1.name)
print(p1.health)
print(statmultiplier())
print(p1.name)
print(p1.attack)
print(statmultiplier())
print(p1.name)
print(p1.defense)
print(statmultiplier())
print(p1.name)
print(p1.health)


print(m1.name)
print(MONSTERSTATS['wolf'][0])

print(statmultiplier())
print(m1.name)
print(m1.health)
print(statmultiplier())
print(m1.name)
print(m1.attack)
print(statmultiplier())
print(m1.name)
print(m1.defense)
print(statmultiplier())
print(m1.name)
print(m1.health)
'''