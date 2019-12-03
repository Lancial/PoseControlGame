from flask import Flask, jsonify, request
import json
app = Flask(__name__)

label_dict = {'STAND': 0, 'RUN': 1, 'JUMP_UP': 2, 'JUMP_LEFT': 3,
              'JUMP_RIGHT': 4, 'STAND_ATTACK': 5, 'ATTACK': 6}


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
        return jsonify({"pose": -1})  # a place holder for now
    else:
        # return "not a pose" if data format is wrong
        return jsonify({"pose": -1})


if __name__ == '__main__':
    app.run(debug=True)
