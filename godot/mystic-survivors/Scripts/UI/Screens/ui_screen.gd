extends CanvasLayer
class_name UI_Screen

@onready var level_label = %LevelLabel
@onready var time_label = %TimeLabel
@onready var health_label = %HealthLabel
@onready var move_speed_label = %MoveSpeedLabel
@onready var damage_label = %DamageLabel
@onready var duration_label = %DurationLabel
@onready var knockback_label = %KnockbackLabel
@onready var pierce_label = %PierceLabel
@onready var projectile_speed_label = %ProjectileSpeedLabel
@onready var cooldown_label = %CooldownLabel
@onready var size_label = %SizeLabel
@onready var regen_label = %RegenLabel
@onready var magnet_label = %MagnetLabel
@onready var amount_label = %AmountLabel

func update_screen(player: Player, ui_main: CanvasLayer) -> void:
	var level_component: LevelComponent = player.level_component
	var stats_component: StatsComponent = player.stats_component
	
	var experience_bar = "( %d / %d )" % [level_component.experience, level_component.experience_cap] 
	level_label.text = "Level: [hint=%s]%d[/hint]" % [experience_bar, level_component.level]
	
	time_label.text = "Current time: %s" % ui_main.get_time_string()
	
	# GENERAL STATS ============================================================
	var current_stat = stats_component.get_health()
	var base_stat = stats_component.base_stats.health
	health_label.text = apply_quality("Max Health: %s", current_stat, base_stat)
	
	current_stat = stats_component.get_move_speed()
	base_stat = stats_component.base_stats.move_speed
	move_speed_label.text = apply_quality("Move Speed: %s", current_stat, base_stat)
	
	current_stat = stats_component.get_damage() * 100
	base_stat = stats_component.base_stats.damage * 100
	damage_label.text = apply_quality("Damage: %s", current_stat, base_stat)
	
	current_stat = stats_component.get_duration() * 100
	base_stat = stats_component.base_stats.duration * 100
	duration_label.text = apply_quality("Duration: %s", current_stat, base_stat)
	
	current_stat = stats_component.get_knockback() * 100
	base_stat = stats_component.base_stats.knockback * 100
	knockback_label.text = apply_quality("Knockback: %s", current_stat, base_stat)
	
	current_stat = (stats_component.get_pierce() + 1) * 100
	base_stat = (stats_component.base_stats.pierce + 1) * 100
	pierce_label.text = apply_quality("Pierce: %s", current_stat, base_stat)
	
	current_stat = stats_component.get_projectile_speed() * 100
	base_stat = stats_component.base_stats.projectile_speed * 100
	projectile_speed_label.text = apply_quality("Proj. Speed: %s", current_stat, base_stat)
	
	current_stat = stats_component.get_cdr() * 100
	base_stat = stats_component.base_stats.cdr * 100
	cooldown_label.text = apply_quality("Cooldown: %s", current_stat, base_stat)
	
	current_stat = stats_component.get_size() * 100
	base_stat = stats_component.base_stats.size * 100
	size_label.text = apply_quality("Size: %s", current_stat, base_stat)
	
	current_stat = stats_component.get_regen() * 100
	base_stat = stats_component.base_stats.regen * 100
	regen_label.text = apply_quality("Regen: %s", current_stat, base_stat)
	
	current_stat = stats_component.get_magnet() * 100
	base_stat = stats_component.base_stats.magnet * 100
	magnet_label.text = apply_quality("Magnet: %s", current_stat, base_stat)
	
	current_stat = stats_component.get_amount() * 100
	base_stat = stats_component.base_stats.amount * 100
	amount_label.text = apply_quality("Amount: %s", current_stat, base_stat)
	

func apply_quality(description: String, current_stat, base_stat) -> String:
	var color = quality_color(current_stat, base_stat)
	#print(description, color)
	return (description % color)

func quality_color(current_stat, base_stat) -> String:
	var value = current_stat / base_stat
	
	var color = "[color=%s] %d %% [/color]"
	
	var common = 'white'
	var uncommon =  'green'
	var rare =  'cyan'
	var epic = 'red'
	var legendary = 'purple'
	var exquisite = 'yellow'
	
	if value < 1.5:
		return color % [common, value * 100]
	elif value < 2.2:
		return color % [uncommon, value * 100]
	elif value < 3.1:
		return color % [rare, value * 100]
	elif value < 4.5:
		return "[shake]" + color % [epic, value * 100] + "[/shake]"
	elif value < 7:
		return "[wave amp=15 freq=5]" + color % [legendary, value * 100] + "[/wave]"
	else:
		return "[tornado radius=1 freq=4][rainbow]" + color % [exquisite, value * 100] + "[/rainbow][/tornado]"

func _on_quit_button_pressed():
	get_tree().paused = false
	reset_objects()
	get_tree().change_scene_to_file("res://Scenes/Menu/main_menu.tscn")

func _on_restart_pressed():
	get_tree().paused = false
	reset_objects()
	get_tree().reload_current_scene()

func reset_objects():
	for projectile in get_tree().get_nodes_in_group("projectiles"):
		projectile.queue_free()
		
	for pickup in get_tree().get_nodes_in_group("pickups"):
		pickup.queue_free()
