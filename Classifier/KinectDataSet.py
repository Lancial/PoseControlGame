import os
import torch
import pandas as pd
import numpy as np
from torch.utils.data import Dataset, DataLoader
from torchvision import transforms, utils


def numeric_label(label):
    return label_dict.get(label, -1)


class KinectDataSet(Dataset):

    def __init__(self, csv_file, root_dir, transform=None):
        self.skeleton_data = pd.read_csv(csv_file, header=None)
        # the first column contains label

        self.num_label = np.asarray(self.skeleton_data.iloc[:, 0])
        self.transform = transform

    def __len__(self):
        return len(self.skeleton_data.index)

    def __getitem__(self, idx):
        if torch.is_tensor(idx):
            idx = idx.tolist()

        single_label = self.num_label[idx]
        # pose_label = numeric_label(self.pose_array)
        joints_set = np.asarray(
            self.skeleton_data.iloc[idx][1:].astype('float'))
        sample = (torch.from_numpy(joints_set).float(),
                  single_label)

        return sample
