extends Node2D
class_name Drop

var drop_rate: float = 0
var value: float = 0
var pickup_scene: Resource

func _init(drop_rate: float, value: float, pickup_scene: Resource):
	self.drop_rate = drop_rate
	self.value = value
	self.pickup_scene = pickup_scene
