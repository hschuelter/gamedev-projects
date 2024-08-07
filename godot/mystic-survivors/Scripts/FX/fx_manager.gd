extends Node2D

@onready var screen_size = get_viewport().get_visible_rect().size

var cloud_scene: Resource = preload("res://Scenes/FX/fx_clouds.tscn")
var leaf_scene: Resource = preload("res://Scenes/FX/fx_leaf.tscn")

@onready var timer_cloud = $timer_cloud
@onready var timer_leaf = $timer_leaf

func _ready():
	timer_cloud.start()

func instantiate_fx(scene: Resource, number: int) -> void:
	for i in range(0, number):
		var fx = scene.instantiate()
		fx.global_position = random_position()
		get_tree().root.add_child(fx)

func random_position() -> Vector2:
	var rng = RandomNumberGenerator.new()
	var pos_x = rng.randi_range( -screen_size.x, screen_size.x / 2)
	var pos_y = rng.randi_range( -screen_size.y, screen_size.y / 2)
	return Vector2(pos_x, pos_y) + self.global_position

func _on_timer_cloud_timeout() -> void:
	instantiate_fx(cloud_scene, 2)

func _on_timer_leaf_timeout() -> void:
	instantiate_fx(leaf_scene, 30)
