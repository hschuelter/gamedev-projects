extends Node
class_name EnemyAttack

@export var cooldown_duration: float = 2.0

func Attack(player: Player, enemy: Enemy):
	print("Attack!!!!")
