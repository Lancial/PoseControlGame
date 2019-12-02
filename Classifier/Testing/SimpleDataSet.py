import os
import torch
import pandas as pd
import numpy as np
from torch.utils.data import Dataset, DataLoader
from torchvision import transforms, utils
label_dict = {'STAND': 0, 'RUN': 1, 'JUMP_UP': 2, 'JUMP_LEFT': 3,
              'JUMP_RIGHT': 4, 'STAND_ATTACK': 5, 'ATTACK': 6}


def numeric_label(label):
    return label_dict.get(label, -1)


class SimpleDataSet(Dataset):

    def __init__(self, csv_file, root_dir, transform=None):
        self.skeleton_data = pd.read_csv(csv_file, header=None)
        # the first column contains label
        self.pose_array = self.skeleton_data.iloc[:, 0].tolist()
        self.num_label = np.asarray(numeric_label(self.pose_array))
        self.joints_set = np.asarray(self.skeleton_data.iloc[:, 1:])
        self.transform = transform

    def __len__(self):
        return len(self.skeleton_data)

    def __getitem__(self, idx):
        if torch.is_tensor(idx):
            idx = idx.tolist()

        # pose_label = numeric_label(self.pose_array)
        joints_set = self.joints_set.astype('float')
        sample = (torch.from_numpy(joints_set).long(),
                  torch.from_numpy(self.num_label))

        return sample
