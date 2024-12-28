import numpy as np
import cv2
import dlib

# dlib yüz tanıyıcıyı yükleyin
detector = dlib.get_frontal_face_detector()
predictor = dlib.shape_predictor('utils/shape_predictor_68_face_landmarks.dat')

def overlay_hair(image_path, hair_path, output_path):
    # Yüklenen fotoğraf
    image = cv2.imread(image_path)
    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)

    # Yüz algılama
    faces = detector(gray)
    if len(faces) == 0:
        return False  # Yüz bulunamadı

    # İlk yüz için işlem yapıyoruz
    face = faces[0]

    # Yüz hatlarını al
    landmarks = predictor(gray, face)
    landmarks_array = np.array([[p.x, p.y] for p in landmarks.parts()])

    # Alın ve çene arasındaki mesafeyi hesapla
    forehead_x, forehead_y = landmarks_array[19]
    chin_x, chin_y = landmarks_array[8]
    face_width = np.linalg.norm(landmarks_array[0] - landmarks_array[16])  # Yüz genişliği

    # Saç resmini yükle
    hair = cv2.imread(hair_path, cv2.IMREAD_UNCHANGED)  # Alfa kanalı için IMREAD_UNCHANGED

    # Saç boyutunu yeniden ayarla
    hair_height, hair_width = hair.shape[:2]
    scale_factor = face_width / hair_width
    new_hair_width = int(hair_width * scale_factor)
    new_hair_height = int(hair_height * scale_factor)
    hair = cv2.resize(hair, (new_hair_width, new_hair_height))

    # Saçı görüntünün üzerine yerleştir
    y_offset = max(0, forehead_y - new_hair_height)
    x_offset = max(0, (forehead_x - new_hair_width // 2)+75)
    for c in range(3):  # RGB kanalları
        alpha = hair[:, :, 3] / 255.0
        image[y_offset:y_offset + new_hair_height, x_offset:x_offset + new_hair_width, c] = (
            alpha * hair[:, :, c] + (1 - alpha) * image[y_offset:y_offset + new_hair_height, x_offset:x_offset + new_hair_width, c]
        )

    # Sonuç resmini kaydet
    cv2.imwrite(output_path, image)
    return True
