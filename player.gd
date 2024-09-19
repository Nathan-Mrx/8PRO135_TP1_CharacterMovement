extends CharacterBody2D

@onready var sprite: AnimatedSprite2D = $sprite

const SPEED = 80.0

func _physics_process(delta: float) -> void:
	var direction_x := Input.get_axis("ui_left", "ui_right")
	var direction_y := Input.get_axis("ui_up", "ui_down")

	if direction_x:
		velocity.x = direction_x * SPEED
		sprite.play("walk_right_left")
		sprite.flip_h = direction_x < 0
	else:
		velocity.x = move_toward(velocity.x, 0, SPEED)

	if direction_y:
		velocity.y = direction_y * SPEED
		if direction_y > 0:
			sprite.play("walk_down")
		else:
			sprite.play("walk_up")
	else:
		velocity.y = move_toward(velocity.y, 0, SPEED)

	if direction_x == 0 and direction_y == 0:
		sprite.play("idle")

	move_and_slide()
