extends CanvasLayer

@onready var fps_label = $MetaContainer/FpsLabel
@onready var time_label = $MetaContainer/TimeLabel
var time: float = 0
@onready var kill_count_label = $MetaContainer/KillCountLabel
@onready var current_wave_label = $MetaContainer/CurrentWaveLabel

@onready var health_bar = %HealthBar

@onready var experience_bar = %ExperienceBar
@onready var label_level = %Label_Level

@onready var move_speed_value = %MoveSpeedValue
@onready var damage_value = %DamageValue
@onready var duration_value = %DurationValue
@onready var knockback_value = %KnockbackValue
@onready var pierce_value = %PierceValue
@onready var projectile_speed_value = %ProjectileSpeedValue
@onready var cdr_value = %CDRValue
@onready var size_value = %SizeValue

func _process(delta: float) -> void:
	time += delta
	fps_label.text = "FPS: " + str(Engine.get_frames_per_second())
	time_label.text = get_time_string()

func update_ui(stats: StatsPlayer) -> void:
	move_speed_value.text = str(stats.move_speed)
	damage_value.text = str(stats.damage) 
	duration_value.text = str(stats.duration) 
	knockback_value.text = str(stats.knockback) 
	pierce_value.text = str(stats.pierce) 
	projectile_speed_value.text = str(stats.projectile_speed) 
	cdr_value.text = str(stats.cdr) 
	size_value.text = str(stats.size) 

func update_hp(value: float) -> void:
	health_bar.scale.x = value

func update_kill_count(value: int) -> void:
	kill_count_label.text = "K: " + str(value)

func update_wave_ui(value: int) -> void:
	current_wave_label.text = "Wave " + str(value + 1)

func get_time_string() -> String:
	var seconds: int
	var minutes: int
	var text: String = "%02d:%02d"
	
	minutes = floor(time / 60)
	seconds = fmod(floor(time), 60)
	
	return text % [minutes, seconds]
