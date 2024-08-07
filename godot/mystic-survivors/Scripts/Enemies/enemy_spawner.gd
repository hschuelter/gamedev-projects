extends Node2D

@onready var world = $".."
@export var disable: bool = false
@export var player: Player
@export var waves_list: Array = []
var current_wave: Wave
var current_wave_value: int = 0

var enemy_count: int = 0
var kill_count: int = 0
var spawn_points_array: Array = []
var can_spawn: bool = true

signal wave_change
signal enemy_killed

@onready var spawn_cooldown_timer: Timer = $SpawnCooldownTimer
@onready var wave_timer = $WaveTimer

var rng = RandomNumberGenerator.new()

func _ready():
	self.global_position = player.global_position
	create_spawn_points_array()
	current_wave = waves_list[0]
	spawn_cooldown_timer.wait_time = current_wave.spawn_cooldown
	wave_timer.wait_time = current_wave.wave_duration
	wave_timer.start()

func _process(delta):
	self.global_position = player.global_position
	if can_spawn and not disable:
		spawn_enemies()

func spawn_enemies() -> void:
	for i in range(current_wave.spawn_rate):
		if (enemy_count >= current_wave.spawn_limit):
			break
		var position: Vector2 = self.global_position + get_random_position()
		var enemy: Resource = get_random_enemy()
		spawn_single_enemy(enemy, position)
	
	can_spawn = false

func _on_spawn_cooldown_timer_timeout() -> void:
	can_spawn = true

func change_wave(num: int) -> void:
	current_wave = waves_list[num]
	spawn_cooldown_timer.wait_time = current_wave.spawn_cooldown
	wave_timer.wait_time = current_wave.wave_duration
	wave_timer.start()
	can_spawn = true

func spawn_single_enemy(enemy: Resource, position: Vector2) -> void:	
	var spawned_enemy: Enemy = enemy.instantiate()
	spawned_enemy.global_position = position
	spawned_enemy.player = player
	spawned_enemy.name = "Enemy"
	world.add_child(spawned_enemy)
	spawned_enemy.connect("mob_died", on_mob_died)
	spawned_enemy.connect("mob_respawn", respawn)
	enemy_count += 1

#region Auxiliary Functions
func create_spawn_points_array() -> void:
	spawn_points_array = [
		Vector2(   0, -150),
		Vector2(   0,  150),
		Vector2( 240,    0),
		Vector2(-240,    0),
		Vector2( 240,  150),
		Vector2(-240,  150),
		Vector2( 240, -150),
		Vector2(-240, -150)
	]
	
func create_spawn_point(x: int, y: int) -> Marker2D:
	var point: Marker2D = Marker2D.new()
	point.global_position = Vector2(x, y)
	self.add_child(point)
	return point

func get_random_position() -> Vector2:
	var num = rng.randi_range(0, spawn_points_array.size() - 1)
	return spawn_points_array[num]

func get_random_enemy() -> Resource:
	var num = rng.randi_range(0, current_wave.enemies_list.size() - 1)
	return current_wave.enemies_list[num]
#endregion

#region Signals ================================================================
func on_mob_died() -> void:
	kill_count  += 1
	enemy_count -= 1
	emit_signal("enemy_killed", kill_count)
	
func respawn(enemy) -> void:
	var position: Vector2 = self.global_position + get_random_position()
	enemy.global_position = position
#endregion

func _on_wave_timer_timeout() -> void:
	emit_signal("wave_change", current_wave_value + 1)
	current_wave_value = clamp(current_wave_value + 1, 0, waves_list.size()-1)
	change_wave(current_wave_value)
