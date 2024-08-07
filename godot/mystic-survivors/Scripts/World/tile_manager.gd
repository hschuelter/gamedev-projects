extends Node2D

@export var player: Player
var map: Dictionary
var tilemap_scene: Resource = preload("res://Scenes/Tilemap/terrain.tscn")

var objects = [
	preload("res://Scenes/Objects/object_plants.tscn"),
	preload("res://Scenes/Objects/object_tree_trunk.tscn")
]

var offset_array = [
	Vector2( 1,  0),
	Vector2( 1,  1),
	Vector2( 0,  1),
	Vector2(-1,  1),
	Vector2(-1,  0),
	Vector2(-1, -1),
	Vector2( 0, -1),
	Vector2( 1, -1)
]

func _ready():
	var tile = tilemap_scene.instantiate()
	tile.global_position = Vector2.ZERO
	tile.name = "Tile (0,0)"
	map[Vector2(0,0)] = tile
	add_child(tile)

func _process(delta) -> void:
	var x = round(player.global_position.x / 512)
	var y = round(player.global_position.y / 512)
	var current_tile = Vector2(x, y)

	for offset in offset_array:
		var adjacent = current_tile + offset
		if not map.has(adjacent):
			var tile = tilemap_scene.instantiate()
			tile.global_position = adjacent * 512
			tile.name = "Tile " + str(adjacent)
			map[adjacent] = tile
			add_child(tile)
			
			for p in tile.positions:
				var index = randf() * objects.size()
				var obj = objects[index].instantiate()
				obj.global_position = p.global_position
				tile.add_child(obj)
