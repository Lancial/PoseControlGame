
import json
import torch
import time
from flask import Flask, jsonify, request
from os import path

path_directory = path.join(path.dirname(
    path.realpath(__file__)), './../Classifier')

from path.join(path_directory, '/net') import Net

app = Flask(__name__)


# initialize the model
PATH = path.join(path_directory, '/model.pth')
model = Net(*args, **kwargs)
model.load_state_dict(torch.load(PATH))
model.eval()

# map from number to actual pose in str
label_dict = {0:'STAND', 1:'RUN', 2:'JUMP_UP', 3:'JUMP_LEFT',
              4:'JUMP_RIGHT', 5:'STAND_ATTACK', 6:'ATTACK'}


@app.route('/')
def index():
    print('RECEIVE A CONNECT')
    return 'hello'

# check the format of input
def check_format(skeleton):
    return len(skeleton) == 75

# plug data into the model
def get_inference(skeleton):
    start_time = time.time()
    data = skeleton  # need some processing maybe
    output = model(data)
    prediction = torch.argmax(output)
    start_time = time.time()
    elapsed_time = time.time() - start_time
    return prediction, elapsed_time

# return a json str, pose ranges from 0 - 6
# refer to label dict for pose classes
@app.route('/get_inference', methods='POST')
def getPose():
    skeleton = json.loads(request.get_json)[
        'joint_set']  # change this name later
    if check_format(skeleton):
        # here call our neural net
        prediction, time = get_inference(skeleton)  # also need processing
        print('Pose predicted: ' + label_dict[prediction] + ' took ' + time)
        return jsonify({"pose": -1}), 201  # a place holder for now
    else:
        # return "not a pose" if data format is wrong
        print('Pose predicted: not a pose')
        return jsonify({"pose": -1}), 202


if __name__ == '__main__':
    app.run(debug=True)
