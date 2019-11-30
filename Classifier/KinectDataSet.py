import os
import torch
import pandas as pd
import numpy as np
from torch.utils.data import Dataset, DataLoader
from torchvision import transforms, utils


class KinectDataSet(Dataset):

    def __init__(self, csv_file, root_dir, transform=None):
        self.skeleton_data = pd.read_csv(csv_file)
        self.root_dir = root_dir
        self.transform = transform

    def __len__(self):
        return len(self.skeleton_data)

    def numeric_label(label):
        label_dict = {'STAND': 0, 'RUN': 1, 'JUMP_UP': 2, 'JUMP_LEFT': 3,
                      'JUMP_RIGHT': 4, 'STAND_ATTACK': 5, 'ATTACK': 6}
        return label_dict.get(label, -1)

    def __getitem__(self, idx):
        if torch.is_tensor(idx):
            idx = idx.tolist()

        pose_class = os.path.join(self.root_dir,
                                  self.skeleton_data.iloc[idx, 0])
        joints_set = self.skeleton_data.iloc[idx, 1:]
        joints_set = joints_set.astype('float')
        sample = {'pose': torch.from_numpy(
            pose_class), 'joints': torch.from_munpy(joints_set)}

        if self.transform:
            sample = self.transform(sample)

        return sample
