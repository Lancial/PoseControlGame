import torch
import torchvision
from torchvision import transforms, datasets
import torch.nn as nn
import torch.nn.functional as F
import torch.optim as optim
from os import path
from SimpleDataSet import SimpleDataSet

class ToTensor(object):
    def __call__(self, sample):
        pose, joint_set = sample['pose_label'], sample['skeleton_data']
        return {'pose_label': torch.Tensor(pose), 'skeleton_data': torch.Tensor(joint_set)}

csv_directory = path.dirname(path.realpath(__file__))
csv_path = path.join(csv_directory, 'skeleton_experiment.csv')

train = SimpleDataSet(csv_file=csv_path, root_dir=csv_path, transform=transforms.Compose([ToTensor()]))
trainset = torch.utils.data.DataLoader(train, batch_size=10, shuffle=True)


for data in trainset:
    X, y = data['skeleton_data'], data['pose_label']
    print(X)
    print(y)