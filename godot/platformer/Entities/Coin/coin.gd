extends Area2D
class_name Coin

const SPEED = 60
const DISTANCE = 5

var initial_position : Vector2
var target : Node2D

func _ready() -> void:
	initial_position = self.position

func _process(delta: float) -> void:
	var move_position = initial_position
	if target:
		move_position = target.global_position
	
	follow(move_position, delta)

func follow(target: Vector2, delta: float) -> void:
		#print(target.global_position)
		var dif = target - self.global_position
		if dif.length() > DISTANCE:
			position += dif.normalized() * SPEED * delta

func _on_body_entered(player: Player) -> void:
	if not target:
		target = player.target
	if not self in player.coins_collected:
		player.coins_collected.append(self)
		
