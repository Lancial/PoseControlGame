from path.join(path_directory, '/net') import Net
import json
import torch
from flask import Flask, jsonify, request
from os import path

path_directory = path.join(path.dirname(
    path.realpath(__file__)), './../Classifier')


app = Flask(__name__)
PATH = path.join(path_directory, '/model.pth')

# initialize the model
model = Net(*args, **kwargs)
model.load_state_dict(torch.load(PATH))
model.eval()


def check_format(skeleton):
    return len(skeleton) == 75


# return a json str, pose ranges from 0 - 6
# refer to label dict for pose classes
@app.route('/', methods='POST')
def getPose():
    skeleton = json.loads(request.get_json)[
        'joint_set']  # change this name later
    if check_format(skeleton):
        # here call our neural net
        get_inference(skeleton)  # also need processing
        return jsonify({"pose": -1})  # a place holder for now
    else:
        # return "not a pose" if data format is wrong
        return jsonify({"pose": -1})


def get_inference(skeleton):
    data = skeleton  # need some processing maybe
    output = model(data)
    prediction = torch.argmax(output)
    return prediction


if __name__ == '__main__':
    app.run(debug=True)
