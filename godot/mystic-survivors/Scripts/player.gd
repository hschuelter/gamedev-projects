extends CharacterBody2D
class_name Player

@export var _movement_data: PlayerMovementData
@export var base_stats: StatsPlayer
var last_input_vector: Vector2 = Vector2.DOWN

@onready var sprite_main = $sprite_main
@onready var animation_player = $AnimationPlayer
@onready var hit_flash_animation_player = $HitFlashAnimationPlayer
@onready var animation_tree = $AnimationTree
@onready var animation_state = animation_tree.get("parameters/playback")
@onready var footstep_audio = %FootstepAudio
@onready var hit_audio = $PlayerAudio/HitAudio

@onready var hurtbox = $Hurtbox
@onready var pickup_range = $PickupRange

@onready var health_component = %HealthComponent
@onready var level_component = %LevelComponent
@onready var stats_component = %StatsComponent

@onready var inventory = $Inventory
@onready var weapons = %Weapons
@export var passive_items: Array = []

signal level_up
signal game_over

func _ready() -> void:
	stats_component.base_stats = base_stats
	health_component.max_health = stats_component.get_health()
	health_component.current_health = stats_component.get_health()
	
	level_component.experience = 0
	level_component.experience_cap = 100
	
	#var starting_weapon = SkillBow.new()
	var weapon_node = inventory.instantiate_weapon()
	weapon_node.stats_component = stats_component
	weapons.add_child(weapon_node)

func _physics_process(delta: float) -> void:
	var input_vector = get_input_vector()
	regen(delta)
	
	handle_movement(input_vector, delta)
	handle_animations(input_vector)
	move_and_slide()

func regen(delta: float) -> void:
	health_component.heal(stats_component.get_regen() * delta * 0.1)

func handle_movement(input_vector: Vector2, delta: float) -> void:
	if input_vector:
		velocity = input_vector.normalized() * stats_component.get_move_speed()
		last_input_vector = input_vector
	else:
		velocity = velocity.move_toward(Vector2.ZERO, _movement_data.friction * delta)

func get_input_vector() -> Vector2:
	var input_vector = Vector2.ZERO
	input_vector.x = Input.get_axis("ui_left", "ui_right")
	input_vector.y = Input.get_axis("ui_up", "ui_down")
	return input_vector

func add_weapon(weapon_data: WeaponData, weapon_scene: Resource):
	var weapon_found: Skill = find_weapon(weapon_data)
	if not weapon_found:
		var weapon_node = weapon_scene.instantiate()
		weapon_node.stats_component = stats_component
		weapon_node.name = weapon_data.title
		weapons.add_child(weapon_node)
	else:
		upgrade_weapon(weapon_found)

func find_weapon(weapon_data: WeaponData) -> Skill:
	for w in weapons.get_children():
		if w.name == weapon_data.title:
			return w
	
	return null

func upgrade_weapon(weapon: Skill) -> void:
	weapon.level_up()

func add_item(item: PassiveItem):
	var item_found: PassiveItem = find_item(item)
	if not item_found:
		item.stats = stats_component
		item.apply_modifier()
		passive_items.push_back(item)
	else:
		upgrade_item(item_found)
	
	health_component.max_health = stats_component.get_health()
	pickup_range.scale = Vector2(stats_component.get_magnet(), stats_component.get_magnet())

func find_item(item: PassiveItem) -> PassiveItem:
	for i in range(0, passive_items.size()):
		if (item.title == passive_items[i].title):
			return passive_items[i]
	
	return null

func upgrade_item(item: PassiveItem) -> void:
	item.apply_modifier()

func take_damage(value: float) -> void:
	health_component.damage(value)
	hit_flash_animation_player.play("hit_flash")
	play_hit_effect()
	
func play_hit_effect() -> void:
	hit_audio.pitch_scale = randf_range(.8, 1.2)
	hit_audio.play()

func gain_exp(value: float) -> void:
	level_component.experience += value

func _on_hurtbox_area_entered(area: Area2D) -> void:
	#hurtbox.enable_hurtbox()
	pass

func _on_health_component_die() -> void:
	emit_signal("game_over")

func _on_level_component_player_level_up():
	emit_signal("level_up")

func handle_animations(input_vector: Vector2) -> void:
	if input_vector:
		animation_tree.set("parameters/Idle/blend_position", input_vector)
		animation_tree.set("parameters/Move/blend_position", input_vector)
		animation_state.travel("Move")
	else:
		animation_tree.set("parameters/Idle/blend_position", last_input_vector)
		animation_tree.set("parameters/Move/blend_position", last_input_vector)
		animation_state.travel("Idle")

func play_footstep_audio() -> void:
	footstep_audio.pitch_scale = randf_range(0.8, 1.2)
	footstep_audio.play(.05)
