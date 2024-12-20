call venv/scripts/activate
yolo task=detect mode=train imgsz=640 data=train_aimmy.yaml epochs=1000 batch=16 name=Train
cmd