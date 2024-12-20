python -m venv venv
call venv/scripts/activate
.\venv\Scripts\python.exe -m pip install --upgrade pip
pip install ultralytics
pip uninstall torch torchvision
pip3 install torch torchvision --index-url https://download.pytorch.org/whl/cu118
pip install onnx
pip install tensorboard
cmd
