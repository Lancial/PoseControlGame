import torch
import torchvision
from torchvision import transforms, datasets
import torch.nn as nn
import torch.nn.functional as F
import torch.optim as optim
from os import path
from SimpleDataSet import SimpleDataSet


csv_directory = path.dirname(path.realpath(__file__))
csv_path = path.join(csv_directory, 'skeleton_experiment.csv')

train = SimpleDataSet(csv_file=csv_path, root_dir=csv_path, transform=transforms.Compose([transforms.ToTensor()]))
trainset = torch.utils.data.DataLoader(train, batch_size=10, shuffle=True)


for data in trainset:
    X, y = data['skeleton_data'], data['poss_label']
    print(X)
    print(y)