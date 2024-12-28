import dlib
import cv2
import numpy as np

# dlib yüz tanıyıcıyı yükleyin
detector = dlib.get_frontal_face_detector()
predictor = dlib.shape_predictor('utils/shape_predictor_68_face_landmarks.dat')

def detect_face_shape(image_path):
    # Resmi yükle
    image = cv2.imread(image_path)
    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)

    # Yüzleri tespit et
    faces = detector(gray)

    if len(faces) == 0:
        return None  # Yüz bulunamadı

    for face in faces:
        # Yüz hatlarını tespit et
        landmarks = predictor(gray, face)

        # Yüz hatlarını numpy array'ine dönüştür
        landmarks_array = np.array([[p.x, p.y] for p in landmarks.parts()])

        # Yüz hatlarıyla bazı metrikler hesaplayalım (en/boy oranı, çene şekli vs.)
        chin_length = np.linalg.norm(landmarks_array[8] - landmarks_array[27])  # Çene uzunluğu
        face_width = np.linalg.norm(landmarks_array[0] - landmarks_array[16])  # Yüz genişliği
        face_height = np.linalg.norm(landmarks_array[19] - landmarks_array[8])  # Yüz yüksekliği

        aspect_ratio = face_width / face_height
        if aspect_ratio < 1.0:
            return "yuvarla"
        elif 1.0 <= aspect_ratio < 1.109:
            return "kare"
        elif aspect_ratio >= 1.109:
            return "oval"
        
    return None